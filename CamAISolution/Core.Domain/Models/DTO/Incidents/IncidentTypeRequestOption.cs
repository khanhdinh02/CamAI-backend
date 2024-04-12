using Core.Domain.Models.Attributes;

namespace Core.Domain.DTO;

[Lookup]
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
