using System.Net.Mime;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class IncidentService(
    HttpClient httpClient,
    IBlobService blobService,
    IUnitOfWork unitOfWork,
    IBaseMapping mapping
) : IIncidentService
{
    public async Task<Incident> UpsertIncident(CreateIncidentDto incidentDto)
    {
        var incident = await unitOfWork.Incidents.GetByIdAsync(incidentDto.Id);
        List<Evidence> evidences;
        if (incident == null)
        {
            incident = mapping.Map<CreateIncidentDto, Incident>(incidentDto);
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
        // TODO: make this run in another thread
        var ebInstall = (await unitOfWork.EdgeBoxInstalls.GetAsync(x => x.EdgeBoxId == edgeBoxId)).Values[0];
        var uriBuilder = new UriBuilder
        {
            Host = ebInstall.IpAddress,
            Port = ebInstall.Port,
            Path = "/api/images"
        };
        foreach (var evidence in evidences)
        {
            // TODO: might create a background job to fetch evidence
            try
            {
                uriBuilder.Query = EvidencePathQuery(evidence);
                var stream = await httpClient.GetStreamAsync(uriBuilder.Uri);
                using var binaryReader = new BinaryReader(stream);

                var imageDto = new CreateImageDto
                {
                    ContentType = MediaTypeNames.Image.Png,
                    Filename = evidence.Id.ToString("N"),
                    ImageBytes = binaryReader.ReadBytes((int)stream.Length)
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
    }

    private static string EvidencePathQuery(Evidence evidence) => "path=" + evidence.EdgeBoxPath;
}
