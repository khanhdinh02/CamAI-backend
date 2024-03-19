using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class EdgeBoxInstallProfile : Profile
{
    public EdgeBoxInstallProfile()
    {
        CreateMap<CreateEdgeBoxInstallDto, EdgeBoxInstall>();
        CreateMap<EdgeBoxInstall, EdgeBoxInstallDto>();
    }
}
