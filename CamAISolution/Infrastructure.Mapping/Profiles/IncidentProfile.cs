using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class IncidentProfile : Profile
{
    public IncidentProfile()
    {
        CreateMap<CreateIncidentDto, Incident>();
        CreateMap<Incident, IncidentDto>()
            .ForMember(
                x => x.Evidences,
                opts =>
                    opts.MapFrom(
                        (incident, _, _, ctx) =>
                        {
                            foreach (var evidence in incident.Evidences)
                                evidence.Incident = null!;
                            return ctx.Mapper.Map<ICollection<Evidence>, List<EvidenceDto>>(incident.Evidences);
                        }
                    )
            );
    }
}
