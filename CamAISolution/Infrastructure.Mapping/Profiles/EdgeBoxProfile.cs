using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Infrastructure.Mapping.Profiles;

public class EdgeBoxProfile : Profile
{
    public EdgeBoxProfile()
    {
        CreateMap<EdgeBox, EdgeBoxDto>();
        CreateMap<EdgeBoxStatus, BaseStatusDto>();
        CreateMap<EdgeBoxLocation, BaseStatusDto>();
        CreateMap<CreateEdgeBoxDto, EdgeBox>();
        CreateMap<UpdateEdgeBoxDto, EdgeBox>();
    }
}
