using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Host.CamAI.API;

public class CamAIProfile : Profile
{
    public CamAIProfile()
    {
        CreateMap<Shop, ShopDto>().ForMember(s => s.Status, opts => opts.MapFrom(s => s.ShopStatus));
        CreateMap<ShopStatus, ShopStatusDto>();
        CreateMap<UpdateShopDto, Shop>();
    }
}
