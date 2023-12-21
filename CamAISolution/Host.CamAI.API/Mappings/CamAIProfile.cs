﻿using AutoMapper;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Models.DTOs.Brands;

namespace Host.CamAI.API.Mappings;

public class CamAIProfile : Profile
{
    public CamAIProfile()
    {
        CreateMap<Shop, ShopDto>().ForMember(s => s.Status, opts => opts.MapFrom(s => s.ShopStatus));
        CreateMap<ShopStatus, ShopStatusDto>();
        CreateMap<CreateShopDto, Shop>();
        CreateMap<Shop, ShopDto>();

        CreateMap<Brand, BrandDto>();
        CreateMap<CreateBrandDto, Brand>();
        CreateMap<UpdateBrandDto, Brand>();
        CreateMap<BrandStatus, BrandStatusDto>();
    }
}
