using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class IncidentService(HttpClient httpClient, IUnitOfWork unitOfWork, IBaseMapping mapping) : IIncidentService
{
    public async Task<Incident> CreateIncident(CreateIncidentDto incidentDto)
    {
        var incident = mapping.Map<CreateIncidentDto, Incident>(incidentDto);
        foreach (var evidence in incident.Evidences)
            evidence.Status = EvidenceStatus.ToBeFetched;

        // TODO: how about only add new evidence to existing incident
        incident = await unitOfWork.Incidents.AddAsync(incident);
        if (await unitOfWork.CompleteAsync() > 0)
        {
            // TODO: make this run in another thread
            var ebInstall = (await unitOfWork.EdgeBoxInstalls.GetAsync(x => x.EdgeBoxId == incident.EdgeBoxId)).Values[
                0
            ];
            var uriBuilder = new UriBuilder
            {
                Host = ebInstall.IpAddress,
                Port = ebInstall.Port,
                Path = "/api/images"
            };
            foreach (var evidence in incidentDto.Evidences)
            {
                // TODO: might create a background job to fetch evidence
                // TODO: create evidence Uri link
                uriBuilder.Query = "path=" + evidence.FilePath;
                // TODO: handle exception and update status
                var stream = await httpClient.GetStreamAsync(uriBuilder.Uri);
                // TODO: get base directory to save evidence
                await using var fileStream = new FileStream(evidence.FileName, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
                // TODO: update evidence status
            }
        }

        return incident;
    }
}
