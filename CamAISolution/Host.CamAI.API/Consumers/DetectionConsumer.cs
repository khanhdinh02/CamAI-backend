using Core.Domain.DTO;
using Core.Domain.Interfaces.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_Detection", ConsumerConstant.Detection)]
public class DetectionConsumer(IIncidentService incidentService) : IConsumer<ReceivedIncident>
{
    public async Task Consume(ConsumeContext<ReceivedIncident> context)
    {
        var receivedIncident = context.Message;
        await incidentService.UpsertIncident(Map(receivedIncident));
    }

    private CreateIncidentDto Map(ReceivedIncident receivedIncident)
    {
        return new CreateIncidentDto
        {
            EdgeBoxId = receivedIncident.EdgeBoxId,
            Id = receivedIncident.Id,
            IncidentType = receivedIncident.IncidentType,
            Time = receivedIncident.Time,
            Evidences = receivedIncident.Evidences.Select(Map).ToList()
        };
    }

    private CreateEvidenceDto Map(ReceivedEvidence receivedEvidence)
    {
        return new CreateEvidenceDto
        {
            FilePath = receivedEvidence.FilePath,
            EvidenceType = receivedEvidence.EvidenceType,
            // TODO: sync camera data from edge box to server
            CameraId = receivedEvidence.CameraId
        };
    }
}
