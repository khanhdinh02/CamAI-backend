using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Models.DTOs.Brands;

namespace Core.Domain.Interfaces.Services;

public interface IBrandService
{
    Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest);
    Task<Brand> GetBrandById(Guid id);
    Task<Brand> CreateBrand(Brand brand);
    Task<Brand> UpdateBrand(Guid id, UpdateBrandDto brandDto);
    Task<Brand> ReactivateBrand(Guid id);
    Task UpdateLogo();
    Task UpdateBanner();
    Task DeleteBrand(Guid id);
}
