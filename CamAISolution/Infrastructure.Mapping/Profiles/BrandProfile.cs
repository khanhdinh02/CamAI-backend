using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandDto>()
            .ForMember(x => x.BrandManager, opts => opts.MapFrom(
                (brand, _, _, ctx) =>
                {
                    if (brand.BrandManager == null)
                        return null;
                    brand.BrandManager.Brand = null;
                    return ctx.Mapper.Map<Account, AccountDto>(brand.BrandManager);
                }
            ));
        CreateMap<CreateBrandDto, Brand>()
            .ForMember(des => des.Banner, opts => opts.Ignore())
            .ForMember(des => des.Logo, opts => opts.Ignore());
        CreateMap<UpdateBrandDto, Brand>();
    }
}
