using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;

namespace Host.CamAI.API;

public class CamAIProfile : Profile
{
    public CamAIProfile()
    {
        CreateMap<Shop, ShopDto>();
    }
}
