using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class EvidenceProfile : Profile
{
    public EvidenceProfile()
    {
        CreateMap<CreateEvidenceDto, Evidence>()
            .ForMember(
                x => x.EdgeBoxPath,
                opts => opts.MapFrom((dto, evidence) => evidence.EdgeBoxPath = dto.FilePath)
            );
    }
}
