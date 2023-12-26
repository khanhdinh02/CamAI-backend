using System.Text.Json;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application;

public class BrandService(IUnitOfWork unitOfWork, IAppLogging<BrandService> logger, IBaseMapping mapping)
    : IBrandService
{
    public async Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest)
    {
        return await unitOfWork.Brands.GetAsync(new BrandSearchSpec(searchRequest));
    }

    public async Task<Brand> GetBrandById(Guid id)
    {
        var brandResult = await unitOfWork.Brands.GetAsync(new BrandByIdRepoSpec(id));
        return brandResult.Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Brand), id);
    }

    public async Task<Brand> CreateBrand(Brand brand)
    {
        // TODO [Duy]: create brand with logo and banner
        brand.BrandStatusId = BrandStatusEnum.Active;
        await unitOfWork.Brands.AddAsync(brand);
        await unitOfWork.CompleteAsync();
        logger.Info($"Create new brand: {JsonSerializer.Serialize(brand)}");
        return brand;
    }

    public async Task<Brand> UpdateBrand(Guid id, UpdateBrandDto brandDto)
    {
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand is null)
            throw new NotFoundException(typeof(Brand), id);
        // TODO [Duy]: divide the AppConstant to multiple constant
        if (brand.BrandStatusId == BrandStatusEnum.Inactive)
            throw new BadRequestException("Cannot modified inactive brand");

        mapping.Map(brandDto, brand);

        brand = unitOfWork.Brands.Update(brand);
        unitOfWork.Complete();
        return brand;
    }

    public async Task<Brand> ReactivateBrand(Guid id)
    {
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
            throw new NotFoundException(typeof(Brand), id);
        if (brand.BrandStatusId != BrandStatusEnum.Active)
            throw new BadRequestException("Brand is already active");

        brand.BrandStatusId = BrandStatusEnum.Active;
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
            brand.BrandManagerId = BrandStatusEnum.Inactive;
            unitOfWork.Brands.Update(brand);
        }

        await unitOfWork.CompleteAsync();
    }

    private bool HasRelatedEntities(Brand brand)
    {
        return brand.BrandManagerId != null;
    }
}
