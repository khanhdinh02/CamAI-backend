using Core.Domain.Enums;
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

public static class IncidentTypeRequestOptionExtension
{
    public static IncidentType[] ToIncidentTypes(this IncidentTypeRequestOption option)
    {
        return option switch
        {
            IncidentTypeRequestOption.Phone => [IncidentType.Phone],
            IncidentTypeRequestOption.Uniform => [IncidentType.Uniform],
            IncidentTypeRequestOption.Interaction => [IncidentType.Interaction],
            IncidentTypeRequestOption.Incident => [IncidentType.Phone, IncidentType.Uniform],
            _ => []
        };
    }
}
