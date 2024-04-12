namespace Core.Domain.DTO;

public enum IncidentTypeRequestOption
{
    Phone = 1,
    Uniform = 2,
    Interaction = 3,

    /// <summary>
    /// Include phone and uniform
    /// </summary>
    Incident = 4
}
