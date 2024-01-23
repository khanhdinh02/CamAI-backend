using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.DTO;

namespace Infrastructure.Mapping.Profiles;

public class ProvinceProfile : Profile
{
    public ProvinceProfile()
    {
        CreateMap<Province, ProvinceDto>();
    }
}
