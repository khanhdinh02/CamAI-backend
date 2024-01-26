using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandDto>();
        CreateMap<Brand, BrandDtoWithoutBrandManager>();
        CreateMap<CreateBrandDto, Brand>();
        CreateMap<UpdateBrandDto, Brand>();
        CreateMap<BrandStatus, LookupDto>();
    }
}
