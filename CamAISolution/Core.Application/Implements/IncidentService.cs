using System.Net.Mime;
using Core.Application.Events;
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
    IBlobService blobService,
    IAccountService accountService,
    IEmployeeService employeeService,
    ICameraService cameraService,
    IEdgeBoxInstallService edgeBoxInstallService,
    IUnitOfWork unitOfWork,
    IBaseMapping mapping,
    IncidentSubject incidentSubject
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
        // if previously accepted then remove employee id
        incident.Status = IncidentStatus.Rejected;
        incident.EmployeeId = null;
        unitOfWork.Incidents.Update(incident);
        await unitOfWork.CompleteAsync();
    }

    public async Task<Incident> UpsertIncident(CreateIncidentDto incidentDto)
    {
        var incident = await unitOfWork.Incidents.GetByIdAsync(incidentDto.Id);

        bool isNewIncident = incident == null;

        var ebInstall = await edgeBoxInstallService.GetLatestInstallingByEdgeBox(incidentDto.EdgeBoxId);
        foreach (var cameraId in incidentDto.Evidences.Select(x => x.CameraId))
            await cameraService.CreateCameraIfNotExist(cameraId, ebInstall!.ShopId);

        await unitOfWork.BeginTransaction();
        if (incident == null)
        {
            incident = mapping.Map<CreateIncidentDto, Incident>(incidentDto);
            incident.ShopId = ebInstall!.ShopId;
            incident.Evidences = [];
            incident = await unitOfWork.Incidents.AddAsync(incident);
        }

        foreach (var evidenceDto in incidentDto.Evidences)
        {
            var evidence = mapping.Map<CreateEvidenceDto, Evidence>(evidenceDto);
            evidence.IncidentId = incident.Id;
            var imageDto = new CreateImageDto
            {
                ContentType = MediaTypeNames.Image.Png,
                Filename = evidence.Id.ToString("N") + ".png",
                ImageBytes = evidenceDto.Content
            };
            evidence.Image = await blobService.UploadImage(imageDto, nameof(Incident), incident.Id.ToString("N"));
            await unitOfWork.Evidences.AddAsync(evidence);
        }

        await unitOfWork.CompleteAsync();
        await unitOfWork.CommitTransaction();
        incidentSubject.Notify(new(incident, isNewIncident));
        return incident;
    }
}
