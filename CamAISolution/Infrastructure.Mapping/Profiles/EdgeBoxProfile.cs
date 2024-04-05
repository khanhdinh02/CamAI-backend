using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class EdgeBoxProfile : Profile
{
    public EdgeBoxProfile()
    {
        CreateMap<EdgeBox, EdgeBoxDto>();
        CreateMap<CreateEdgeBoxDto, EdgeBox>();
        CreateMap<UpdateEdgeBoxDto, EdgeBox>();
        CreateMap<EdgeBoxModel, EdgeBoxModelDto>();
        CreateMap<EdgeBoxActivity, EdgeBoxActivityDto>();
    }
}
