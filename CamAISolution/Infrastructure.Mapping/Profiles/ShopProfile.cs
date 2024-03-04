using AutoMapper;
using Core.Application.Specifications;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<Shop, ShopDto>()
            .ForMember(x => x.ShopManager, opts => opts.MapFrom((shop, _, _, ctx) =>
            {
                if (shop.ShopManager == null)
                    return null;
                shop.ShopManager.ManagingShop = null;
                shop.ShopManager.Brand = null;
                return ctx.Mapper.Map<Account, AccountDto>(shop.ShopManager);
            }));
        CreateMap<CreateOrUpdateShopDto, Shop>();
    }
}
