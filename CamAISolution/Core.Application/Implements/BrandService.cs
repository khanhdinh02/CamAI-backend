using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Repositories.Base;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;

namespace Core.Application.Implements;

// TODO: unit of work and logger
public class BrandService(IRepository<Brand> brandRepository, IUnitOfWork unitOfWork) : IBrandService
{
    public async Task<PaginationResult<Brand>> GetBrands()
    {
        // TODO: filter brands
        return await brandRepository.GetAsync(null);
    }

    public async Task<Brand> GetBrandById(Guid id)
    {
        // TODO: filter brands
        return await brandRepository.GetByIdAsync(id);
    }

    public async Task<Brand> CreateBrand(Brand brand)
    {
        brand.Id = Guid.Empty;
        brand.Status = Brand.Statuses.Active;
        await brandRepository.AddAsync(brand);
        await unitOfWork.CompleteAsync();
        return brand;
    }

    public Task<Brand> UpdateBrand(Brand brand)
    {
        // TODO: can it update the status
        brandRepository.Update(brand);
        unitOfWork.Complete();
        return Task.FromResult(brand);
    }

    public async Task DeleteBrand(Guid id)
    {
        var brand = await brandRepository.GetByIdAsync(id);
        brandRepository.Delete(brand);
    }
}
