using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IBrandService
{
    Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest);
    Task<Brand> GetBrandById(Guid id);
    Task<Brand> CreateBrand(CreateBrandDto brandDto, CreateImageDto? banner = null, CreateImageDto? logo = null);
    Task<Brand> UpdateBrand(Guid id, UpdateBrandDto brandDto);
    Task<Brand> ReactivateBrand(Guid id);
    Task UpdateImage(CreateImageDto imageDto, BrandImageType type);
    Task DeleteBrand(Guid id);
}
