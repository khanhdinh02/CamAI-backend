using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Host.CamAI.API;

public class CamAIProfile : Profile
{
    public CamAIProfile()
    {
        CreateMap<Shop, ShopDto>();
        CreateMap<CreateShopDto, Shop>();
    }
}
