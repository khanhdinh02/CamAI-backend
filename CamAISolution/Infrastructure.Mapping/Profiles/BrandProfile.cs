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
        CreateMap<CreateBrandDto, Brand>()
            .ForMember(des => des.Banner, opts => opts.Ignore())
            .ForMember(des => des.Logo, opts => opts.Ignore());
        CreateMap<UpdateBrandDto, Brand>();
        CreateMap<BrandStatus, LookupDto>();
    }
}
