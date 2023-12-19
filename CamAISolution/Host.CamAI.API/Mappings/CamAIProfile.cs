using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;

namespace Host.CamAI.API;

public class CamAIProfile : Profile
{
    public CamAIProfile()
    {
        CreateMap<Shop, ShopDto>();

        CreateMap<Brand, BrandDto>();
        CreateMap<CreateBrandDto, Brand>();
        CreateMap<UpdateBrandDto, Brand>();
    }
}
