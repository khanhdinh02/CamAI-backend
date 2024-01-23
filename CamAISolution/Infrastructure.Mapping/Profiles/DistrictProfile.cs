using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.DTO;

namespace Infrastructure.Mapping.Profiles;

public class DistrictProfile : Profile
{
    public DistrictProfile()
    {
        CreateMap<District, DistrictDto>();
    }
}
