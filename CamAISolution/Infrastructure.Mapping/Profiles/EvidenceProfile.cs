using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class EvidenceProfile : Profile
{
    public EvidenceProfile()
    {
        CreateMap<CreateEvidenceDto, Evidence>()
            .ForMember(x => x.Camera, opts => opts.Ignore())
            .ForMember(x => x.CameraId, opts => opts.MapFrom((dto, _) => dto.Camera.Id));

        CreateMap<Evidence, EvidenceDto>()
            .ForMember(
                x => x.Incident,
                opts =>
                    opts.MapFrom(
                        (evidence, _, _, ctx) =>
                        {
                            if (evidence.Incident == null)
                                return null;
                            evidence.Incident.Evidences = [];
                            return ctx.Mapper.Map<Incident, IncidentDto>(evidence.Incident);
                        }
                    )
            );
    }
}
