using Core.Domain.Entities;

namespace Core.Application.Events.Args;

public class CreatedOrUpdatedIncidentArgs(Incident incident, bool isNewIncident) : EventArgs;
