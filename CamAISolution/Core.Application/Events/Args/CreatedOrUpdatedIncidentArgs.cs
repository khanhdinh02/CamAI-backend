using Core.Domain.Entities;

namespace Core.Application.Events.Args;

public class CreatedOrUpdatedIncidentArgs(Incident incident, IncidentEventType eventType, Guid sentTo) : EventArgs
{
    public Incident Incident => incident;
    public IncidentEventType EventType => eventType;

    public Guid SentTo => sentTo;
}

public enum IncidentEventType
{
    NewIncident,
    MoreEvidence
}
