using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Infrastructure.Mapping.Profiles;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<Shop, ShopDto>()
            .ForMember(s => s.Status, opts => opts.MapFrom(s => s.ShopStatus));
        CreateMap<ShopStatus, ShopStatusDto>();
        CreateMap<CreateOrUpdateShopDto, Shop>();
        CreateMap<PaginationResult<Shop>, PaginationResult<ShopDto>>();
    }
}
