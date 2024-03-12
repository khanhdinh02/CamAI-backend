using System.Net.Mime;
using Core.Application.Exceptions;
using Core.Application.Specifications.Incidents.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class IncidentService(
    HttpClient httpClient,
    IBlobService blobService,
    IAccountService accountService,
    IEmployeeService employeeService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IUnitOfWork unitOfWork,
    IBaseMapping mapping
) : IIncidentService
{
    public async Task<Incident> GetIncidentById(Guid id, bool includeAll = false)
    {
        var incident =
            (await unitOfWork.Incidents.GetAsync(new IncidentByIdRepoSpec(id, includeAll))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Incident), id);
        var currentAccount = accountService.GetCurrentAccount();
        return currentAccount.Role switch
        {
            Role.BrandManager when currentAccount.BrandId == incident.Shop.BrandId => incident,
            Role.ShopManager when currentAccount.Id == incident.Shop.ShopManagerId => incident,
            _ => throw new NotFoundException(typeof(Incident), id)
        };
    }

    public async Task<PaginationResult<Incident>> GetIncidents(SearchIncidentRequest searchRequest)
    {
        var account = accountService.GetCurrentAccount();
        switch (account.Role)
        {
            case Role.BrandManager:
                searchRequest.BrandId = account.BrandId;
                break;
            case Role.ShopManager:
                searchRequest.BrandId = null;
                searchRequest.ShopId = account.ManagingShop!.Id;
                break;
        }

        var includeShop = account.Role == Role.BrandManager;
        return await unitOfWork.Incidents.GetAsync(new IncidentSearchSpec(searchRequest, includeShop));
    }

    public async Task AssignIncidentToEmployee(Guid id, Guid employeeId)
    {
        // TODO: should we limit the time or have some rule when assigning incident
        var incident = await GetIncidentById(id);
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (incident.ShopId != employee.ShopId)
            throw new BadRequestException("Incident and employee are not in the same shop");
        incident.EmployeeId = employee.Id;
        incident.Status = IncidentStatus.Accepted;
        unitOfWork.Incidents.Update(incident);
        await unitOfWork.CompleteAsync();
    }

    public async Task RejectIncident(Guid id)
    {
        var incident = await GetIncidentById(id);
        incident.Status = IncidentStatus.Rejected;
        incident.EmployeeId = null;
        // if previously accepted then remove employee id
        unitOfWork.Incidents.Update(incident);
        await unitOfWork.CompleteAsync();
    }

    public async Task<Incident> UpsertIncident(CreateIncidentDto incidentDto)
    {
        var incident = await unitOfWork.Incidents.GetByIdAsync(incidentDto.Id);
        List<Evidence> evidences;
        if (incident == null)
        {
            incident = mapping.Map<CreateIncidentDto, Incident>(incidentDto);
            var ebInstall = await edgeBoxInstallService.GetInstallingByEdgeBox(incident.EdgeBoxId);
            incident.ShopId = ebInstall!.ShopId;
            foreach (var evidence in incident.Evidences)
                evidence.Status = EvidenceStatus.ToBeFetched;

            incident = await unitOfWork.Incidents.AddAsync(incident);
            evidences = incident.Evidences.ToList();
        }
        else
        {
            // add new evidence to existed
            evidences = mapping.Map<ICollection<CreateEvidenceDto>, List<Evidence>>(incidentDto.Evidences);
            foreach (var evidence in evidences)
            {
                evidence.IncidentId = incident.Id;
                evidence.Status = EvidenceStatus.ToBeFetched;
                await unitOfWork.Evidences.AddAsync(evidence);
            }
        }

        if (await unitOfWork.CompleteAsync() > 0)
            await FetchEvidenceFromEdgeBox(incidentDto.EdgeBoxId, incidentDto.Id, evidences);

        return incident;
    }

    private async Task FetchEvidenceFromEdgeBox(Guid edgeBoxId, Guid incidentId, List<Evidence> evidences)
    {
        // TODO: [Duy] validate that eb install has port and ip address
        evidences = evidences.Where(x => x.Status == EvidenceStatus.ToBeFetched).ToList();
        if (evidences.Count == 0)
            return;

        // TODO: make this run in another thread
        var ebInstall = (await edgeBoxInstallService.GetInstallingByEdgeBox(edgeBoxId))!;
        var uriBuilder = new UriBuilder
        {
            Host = ebInstall.IpAddress,
            Port = ebInstall.Port!.Value,
            Path = "/api/images"
        };
        foreach (var evidence in evidences)
        {
            // TODO: might create a background job to fetch evidence
            try
            {
                uriBuilder.Query = EvidencePathQuery(evidence);
                var response = await httpClient.GetAsync(uriBuilder.Uri);

                response.EnsureSuccessStatusCode();

                // TODO: get extension from content-type
                var imageDto = new CreateImageDto
                {
                    ContentType = MediaTypeNames.Image.Png,
                    Filename = evidence.Id.ToString("N") + ".png",
                    ImageBytes = await response.Content.ReadAsByteArrayAsync()
                };
                evidence.Image = await blobService.UploadImage(imageDto, nameof(Incident), incidentId.ToString("N"));
                evidence.Status = EvidenceStatus.Fetched;
            }
            catch (Exception)
            {
                evidence.Status = EvidenceStatus.NotFound;
            }

            // TODO: test if we can create new image and update evidence at the same time
            unitOfWork.Evidences.Update(evidence);
        }

        await unitOfWork.CompleteAsync();
    }

    private static string EvidencePathQuery(Evidence evidence) => "path=" + evidence.EdgeBoxPath;
}
