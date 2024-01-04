using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Infrastructure.Mapping.Profiles;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandDto>();
        CreateMap<CreateBrandDto, Brand>();
        CreateMap<UpdateBrandDto, Brand>();
        CreateMap<BrandStatus, BrandStatusDto>();
        CreateMap<PaginationResult<Brand>, PaginationResult<BrandDto>>();
    }
}
