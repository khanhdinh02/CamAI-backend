using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping;

public class WardProfile : Profile
{
    public WardProfile()
    {
        CreateMap<Ward, WardDto>();
    }
}
