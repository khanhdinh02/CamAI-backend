using Core.Domain.DTO;
using Core.Domain.Interfaces.Services;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_Detection", ConsumerConstant.Detection)]
public class DetectionConsumer(IIncidentService incidentService) : IConsumer<ReceivedIncidentMessage>
{
    public async Task Consume(ConsumeContext<ReceivedIncidentMessage> context)
    {
        var receivedIncident = context.Message;
        await incidentService.UpsertIncident(Map(receivedIncident));
    }

    private static CreateIncidentDto Map(ReceivedIncidentMessage receivedIncidentMessage)
    {
        return new CreateIncidentDto
        {
            EdgeBoxId = receivedIncidentMessage.EdgeBoxId,
            Id = receivedIncidentMessage.Id,
            IncidentType = receivedIncidentMessage.IncidentType,
            StartTime = receivedIncidentMessage.StartTime,
            EndTime = receivedIncidentMessage.EndTime,
            Evidences = receivedIncidentMessage.Evidences.Select(Map).ToList()
        };
    }

    private static CreateEvidenceDto Map(ReceivedEvidence receivedEvidence)
    {
        return new CreateEvidenceDto
        {
            Content = receivedEvidence.Content,
            EvidenceType = receivedEvidence.EvidenceType,
            // TODO: sync camera data from edge box to server
            CameraId = receivedEvidence.CameraId
        };
    }
}
