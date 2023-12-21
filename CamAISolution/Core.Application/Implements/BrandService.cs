using System.Text.Json;
using Core.Application.Exceptions;
using Core.Application.Specifications.Brands.Repositories;
using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.DTOs.Brands;

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
        // TODO [Duy]: create with logo and banner
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
        // TODO [Duy]: divide the AppConstant to multiple constant
        if (brand.BrandStatusId == AppConstant.BrandInactiveStatus)
            throw new BadRequestException("Cannot modified inactive brand");

        brand.Name = brandDto.Name;
        brand.Phone = brandDto.Phone;
        brand.Email = brandDto.Email;

        brand = unitOfWork.Brands.Update(brand);
        unitOfWork.Complete();
        return brand;
    }

    public async Task<Brand> ReactivateBrand(Guid id)
    {
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
            throw new NotFoundException(typeof(Brand), id, GetType());
        if (brand.BrandStatusId != AppConstant.BrandActiveStatus)
            throw new BadRequestException("Brand is already active");

        brand.BrandStatusId = AppConstant.BrandActiveStatus;
        brand = unitOfWork.Brands.Update(brand);
        await unitOfWork.CompleteAsync();
        return brand;
    }

    public async Task UpdateLogo()
    {
        // TODO [Duy]: create a new service to upload photo to S3
        throw new NotImplementedException();
    }

    public async Task UpdateBanner()
    {
        // TODO [Duy] : upload photo to S3
        throw new NotImplementedException();
    }

    public async Task DeleteBrand(Guid id)
    {
        // TODO [Duy]: implement more business in here
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
            return;

        if (HasRelatedEntities(brand))
            unitOfWork.Brands.Delete(brand);
        else
        {
            brand.BrandManagerId = AppConstant.BrandInactiveStatus;
            unitOfWork.Brands.Update(brand);
        }

        await unitOfWork.CompleteAsync();
    }

    private static bool HasRelatedEntities(Brand brand)
    {
        return brand.BrandManagerId != null;
    }
}
