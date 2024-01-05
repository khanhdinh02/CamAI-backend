using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Infrastructure.Mapping.Profiles;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<Shop, ShopDto>();
        CreateMap<ShopStatus, BaseStatusDto>();
        CreateMap<CreateOrUpdateShopDto, Shop>();
        CreateMap<PaginationResult<Shop>, PaginationResult<ShopDto>>();
    }
}
