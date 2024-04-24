using System.Text.Json;
using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class BrandService(
    IAccountService accountService,
    IShopService shopService,
    IEdgeBoxService edgeBoxService,
    IUnitOfWork unitOfWork,
    IAppLogging<BrandService> logger,
    IBaseMapping mapping,
    EventManager eventManager,
    IBlobService blobService
) : IBrandService
{
    public async Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest)
    {
        var currentAccount = accountService.GetCurrentAccount();
        if (currentAccount.Role == Role.BrandManager)
            searchRequest.BrandId =
                currentAccount.BrandId ?? throw new BadRequestException("Brand manager does not have brand yet");
        else if (currentAccount.Role == Role.ShopManager)
        {
            var shop = await shopService.GetShopById(currentAccount.ManagingShop!.Id);
            searchRequest.BrandId = shop.BrandId;
        }
        else if (currentAccount.Role != Role.Admin)
            throw new ForbiddenException(currentAccount, typeof(Brand));

        return await unitOfWork.Brands.GetAsync(new BrandSearchSpec(searchRequest));
    }

    public async Task<Brand> GetBrandById(Guid id)
    {
        var currentAccount = accountService.GetCurrentAccount();
        var brand =
            (await unitOfWork.Brands.GetAsync(new BrandByIdRepoSpec(id))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Brand), id);

        if (!await HasAccessToBrand(currentAccount, brand))
            throw new ForbiddenException(currentAccount, brand);

        return brand;
    }

    public async Task<Brand> CreateBrand(CreateBrandDto brandDto, CreateImageDto? banner, CreateImageDto? logo)
    {
        var brand = mapping.Map<CreateBrandDto, Brand>(brandDto);
        if (banner != null)
            brand.BannerId = (await blobService.UploadImage(banner, nameof(Brand), nameof(Brand.Banner))).Id;
        if (logo != null)
            brand.LogoId = (await blobService.UploadImage(logo, nameof(Brand), nameof(Brand.Logo))).Id;
        brand.BrandStatus = BrandStatus.Active;
        try
        {
            await unitOfWork.Brands.AddAsync(brand);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            if (brand.LogoId.HasValue)
                await blobService.DeleteImageInFilesystem(brand.LogoId.Value);
            if (brand.BannerId.HasValue)
                await blobService.DeleteImageInFilesystem(brand.BannerId.Value);
            throw new ServiceUnavailableException("Error occurred");
        }
        logger.Info($"Create new brand: {JsonSerializer.Serialize(brand)}");
        return brand;
    }

    public async Task<Brand> UpdateBrand(Guid id, UpdateBrandDto brandDto)
    {
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand is null)
            throw new NotFoundException(typeof(Brand), id);

        var currentAccount = accountService.GetCurrentAccount();
        if (!IsAccountOwnBrand(currentAccount, brand))
            throw new ForbiddenException(currentAccount, brand);
        if (brand.BrandStatus == BrandStatus.Inactive)
            throw new BadRequestException("Cannot modified inactive brand");

        mapping.Map(brandDto, brand);

        brand = unitOfWork.Brands.Update(brand);
        if (unitOfWork.Complete() > 0)
            eventManager.NotifyBrandChanged(brand);
        return brand;
    }

    public async Task<Brand> ReactivateBrand(Guid id)
    {
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
            throw new NotFoundException(typeof(Brand), id);
        if (brand.BrandStatus == BrandStatus.Active)
            throw new BadRequestException("Brand is already active");

        brand.BrandStatus = BrandStatus.Active;
        brand = unitOfWork.Brands.Update(brand);
        await unitOfWork.CompleteAsync();
        return brand;
    }

    public async Task UpdateImage(CreateImageDto imageDto, BrandImageType type)
    {
        var currentAccount = accountService.GetCurrentAccount();
        if (!IsAccountOwnBrand(currentAccount, currentAccount.Brand))
            throw new BadRequestException("Account doesn't manage any brand");
        var brand = await GetBrandById(currentAccount.BrandId!.Value);

        Guid? oldImageId;
        string imageTypePath;
        switch (type)
        {
            case BrandImageType.Logo:
                oldImageId = brand.LogoId;
                imageTypePath = nameof(Brand.Logo);
                break;
            case BrandImageType.Banner:
                oldImageId = brand.BannerId;
                imageTypePath = nameof(Brand.Banner);
                break;
            default:
                throw new BadRequestException("Unknown image enum");
        }
        var newLogo = await blobService.UploadImage(imageDto, nameof(Brand), imageTypePath);
        UpdateBrandImageId(type, brand, newLogo.Id);
        unitOfWork.Brands.Update(brand);
        try
        {
            if (await unitOfWork.CompleteAsync() > 0)
            {
                if (oldImageId.HasValue)
                    await blobService.DeleteImageInFilesystem(oldImageId.Value);
            }
            else
                await blobService.DeleteImageInFilesystem(newLogo.Id);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            await blobService.DeleteImageInFilesystem(newLogo.Id);
            throw new ServiceUnavailableException("Update Logo failed");
        }
    }

    private static void UpdateBrandImageId(BrandImageType type, Brand brand, Guid imageId)
    {
        switch (type)
        {
            case BrandImageType.Logo:
                brand.LogoId = imageId;
                break;
            case BrandImageType.Banner:
                brand.BannerId = imageId;
                break;
        }
    }

    public async Task DeleteBrand(Guid id)
    {
        var brand = await unitOfWork.Brands.GetByIdAsync(id);
        if (brand == null)
            return;

        if (brand.BrandManagerId == null)
        {
            unitOfWork.Brands.Delete(brand);
            await unitOfWork.CompleteAsync();
            if (brand.BannerId.HasValue)
                await blobService.DeleteImageInFilesystem(brand.BannerId.Value);
            if (brand.LogoId.HasValue)
                await blobService.DeleteImageInFilesystem(brand.LogoId.Value);
        }
        else
        {
            var installedEdgeBoxes = await edgeBoxService.GetEdgeBoxesByBrand(id);
            if (installedEdgeBoxes.Any())
                throw new BadRequestException("Cannot delete brand that has active edge box");

            unitOfWork.Shops.UpdateStatusInBrand(id, ShopStatus.Inactive);
            unitOfWork.Accounts.UpdateStatusInBrand(id, AccountStatus.Inactive);
            brand.BrandStatus = BrandStatus.Inactive;
            unitOfWork.Brands.Update(brand);
            await unitOfWork.CompleteAsync();
        }
    }

    private async Task<bool> HasAccessToBrand(Account account, Brand brand)
    {
        if (IsAccountOwnBrand(account, brand))
            return true;

        return await IsAccountOwnShopRelatedToBrand(account, brand);
    }

    private async Task<bool> IsAccountOwnShopRelatedToBrand(Account account, Brand brand)
    {
        if (account.Role == Role.ShopManager)
        {
            var shop = await shopService.GetShopById(account.ManagingShop!.Id);
            if (shop.BrandId == brand.Id)
                return true;
        }

        return false;
    }

    private static bool IsAccountOwnBrand(Account account, Brand? brand)
    {
        return brand != null
            && (account.Role == Role.Admin || (account.Role == Role.BrandManager && account.Brand!.Id == brand.Id));
    }
}
