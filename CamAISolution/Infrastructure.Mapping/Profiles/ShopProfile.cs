using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<Shop, ShopDto>()
            .ForPath(x => x.ShopManager.ManagingShop, opts => opts.Ignore());
        CreateMap<CreateOrUpdateShopDto, Shop>();
    }
}
