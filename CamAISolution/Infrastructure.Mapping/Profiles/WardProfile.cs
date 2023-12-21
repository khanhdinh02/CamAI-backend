using AutoMapper;
using Core.Domain.DTOs;
using Core.Domain.Entities;

namespace Infrastructure.Mapping;

public class WardProfile : Profile
{
    public WardProfile()
    {
        CreateMap<Ward, WardDto>();
    }
}
