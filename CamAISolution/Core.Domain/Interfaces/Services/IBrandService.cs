using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface IBrandService
{
    Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest);
    Task<Brand> GetBrandById(Guid id);
    Task<Brand> CreateBrand(Brand brand);
    Task<Brand> UpdateBrand(Guid id, UpdateBrandDto brandDto);
    Task DeleteBrand(Guid id);
}
