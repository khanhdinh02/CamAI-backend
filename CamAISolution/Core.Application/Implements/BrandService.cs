using System.Text.Json;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;

namespace Core.Application;

public class BrandService(IUnitOfWork unitOfWork, IAppLogging<BrandService> logger) : IBrandService
{
    public async Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest)
    {
        return await unitOfWork.Brands.GetAsync(new BrandSearchSpec(searchRequest));
    }

    public async Task<Brand> GetBrandById(Guid id)
    {
        var brandResult = await unitOfWork.Brands.GetAsync(new BrandByIdRepoSpec(id));
        return brandResult.Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Brand), id, GetType());
    }

    public async Task<Brand> CreateBrand(Brand brand)
    {
        brand.BrandStatusId = AppConstant.BrandActiveStatus;
        await unitOfWork.Brands.AddAsync(brand);
        await unitOfWork.CompleteAsync();
        logger.Info($"Create new brand: {JsonSerializer.Serialize(brand)}");
        return brand;
    }

    public async Task<Brand> UpdateBrand(Guid id, UpdateBrandDto brandDto)
    {
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand is null)
            throw new NotFoundException(typeof(Brand), id, GetType());
        // TODO:
        if (brand.BrandStatusId == AppConstant.BrandInactiveStatus)
            throw new BadRequestException("Cannot modified inactive brand");
        // TODO: can it update the status
        unitOfWork.Brands.Update(brand);
        unitOfWork.Complete();
        return brand;
    }

    public async Task DeleteBrand(Guid id)
    {
        // TODO [Duy]: implement more business in here
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand != null)
            unitOfWork.Brands.Delete(brand);
    }
}
