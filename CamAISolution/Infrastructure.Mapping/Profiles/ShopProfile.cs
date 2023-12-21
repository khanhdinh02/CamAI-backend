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
        CreateMap<CreateShopDto, Shop>();
        CreateMap<UpdateShopDto, Shop>()
            .ForMember(des => des.Name, opts => opts.Condition(src => src.Name != null))
            .ForMember(des => des.WardId, opts => opts.Condition(src => src.WardId.HasValue))
            .ForMember(des => des.ShopStatusId, opts => opts.Condition(src => src.Status.HasValue))
            .ForMember(des => des.WardId, opts => opts.Condition(src => src.WardId.HasValue));
        CreateMap<PaginationResult<Shop>, PaginationResult<ShopDto>>();
    }
}
