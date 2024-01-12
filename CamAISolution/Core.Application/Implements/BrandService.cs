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
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class BrandService(
    IAccountService accountService,
    IShopService shopService,
    IUnitOfWork unitOfWork,
    IAppLogging<BrandService> logger,
    IBaseMapping mapping
) : IBrandService
{
    public async Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest)
    {
        var currentAccount = await accountService.GetCurrentAccount();
        if (currentAccount.HasRole(RoleEnum.Admin))
            return await unitOfWork.Brands.GetAsync(new BrandSearchSpec(searchRequest));

        Guid brandId;
        if (currentAccount.HasRole(RoleEnum.BrandManager))
            brandId = currentAccount.Brand!.Id;
        else if (currentAccount.HasRole(RoleEnum.ShopManager))
        {
            var shop = await shopService.GetShopById(currentAccount.ManagingShop!.Id);
            brandId = shop.BrandId;
        }
        else
            throw new ForbiddenException(currentAccount, typeof(Brand));

        var brand = await GetBrandById(brandId);
        return new PaginationResult<Brand>
        {
            Values = [brand],
            PageIndex = 0,
            PageSize = 1,
            TotalCount = 1
        };
    }

    public async Task<Brand> GetBrandById(Guid id)
    {
        var currentAccount = await accountService.GetCurrentAccount();
        var brand =
            (await unitOfWork.Brands.GetAsync(new BrandByIdRepoSpec(id))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Brand), id);

        if (!await HasAccessToBrand(currentAccount, brand))
            throw new ForbiddenException(currentAccount, brand);

        return brand;
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

        var currentAccount = await accountService.GetCurrentAccount();
        if (!IsAccountOwnBrand(currentAccount, brand))
            throw new ForbiddenException(currentAccount, brand);
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

        if (!HasRelatedEntities(brand))
            unitOfWork.Brands.Delete(brand);
        else
        {
            brand.BrandStatusId = BrandStatusEnum.Inactive;
            unitOfWork.Brands.Update(brand);
        }

        await unitOfWork.CompleteAsync();
    }

    private bool HasRelatedEntities(Brand brand)
    {
        // return brand.BrandManagerId != null;
        return true;
    }

    private async Task<bool> HasAccessToBrand(Account account, Brand brand)
    {
        if (IsAccountOwnBrand(account, brand))
            return true;

        if (await IsAccountOwnShopRelatedToBrand(account, brand))
            return true;

        return false;
    }

    private async Task<bool> IsAccountOwnShopRelatedToBrand(Account account, Brand brand)
    {
        if (account.HasRole(RoleEnum.ShopManager))
        {
            var shop = await shopService.GetShopById(account.ManagingShop!.Id);
            if (shop.BrandId == brand.Id)
                return true;
        }

        return false;
    }

    private static bool IsAccountOwnBrand(Account account, Brand brand)
    {
        return account.HasRole(RoleEnum.Admin)
            || (account.HasRole(RoleEnum.BrandManager) && account.Brand!.Id == brand.Id);
    }
}
