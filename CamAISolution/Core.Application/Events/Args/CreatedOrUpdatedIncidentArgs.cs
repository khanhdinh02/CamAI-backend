using Core.Domain.Entities;
using Core.Domain.Models.Attributes;

namespace Core.Application.Events.Args;

public class CreatedOrUpdatedIncidentArgs(Incident incident, IncidentEventType eventType, Guid sentTo) : EventArgs
{
    public Incident Incident => incident;
    public IncidentEventType EventType => eventType;

    public Guid SentTo => sentTo;
}

[Lookup]
public enum IncidentEventType
{
    NewIncident,
    MoreEvidence
}
