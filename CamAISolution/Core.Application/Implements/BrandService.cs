using System.Text.Json;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
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
    IBaseMapping mapping,
    IBlobService blobService
) : IBrandService
{
    public async Task<PaginationResult<Brand>> GetBrands(SearchBrandRequest searchRequest)
    {
        var currentAccount = await accountService.GetAccountById(accountService.GetCurrentAccount().Id);
        if (currentAccount.HasRole(RoleEnum.BrandManager))
            searchRequest.BrandId =
                currentAccount.BrandId ?? throw new BadRequestException("Brand manager does not have brand yet");
        else if (currentAccount.HasRole(RoleEnum.ShopManager))
        {
            var shop = await shopService.GetShopById(currentAccount.ManagingShop!.Id);
            searchRequest.BrandId = shop.BrandId;
        }
        else if (!currentAccount.HasRole(RoleEnum.Admin))
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
        brand.BrandStatusId = BrandStatusEnum.Active;
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
        if (brand.BrandStatusId == BrandStatusEnum.Active)
            throw new BadRequestException("Brand is already active");

        brand.BrandStatusId = BrandStatusEnum.Active;
        brand = unitOfWork.Brands.Update(brand);
        await unitOfWork.CompleteAsync();
        return brand;
    }

    public async Task UpdateLogo(CreateImageDto imageDto)
    {
        var currentAccount = accountService.GetCurrentAccount();
        if (!IsAccountOwnBrand(currentAccount, currentAccount.Brand))
            throw new BadRequestException("Account doesn't manage any brand");
        var brand = await GetBrandById(currentAccount.BrandId!.Value);
        var oldLogoId = brand.LogoId;
        var newLogo = await blobService.UploadImage(imageDto, nameof(Brand), nameof(Brand.Logo));
        brand.LogoId = newLogo.Id;
        unitOfWork.Brands.Update(brand);
        if (await unitOfWork.CompleteAsync() > 0 && oldLogoId.HasValue)
            await blobService.DeleteImageInFilesystem(oldLogoId.Value);
    }

    public async Task UpdateBanner(CreateImageDto imageDto)
    {
        var currentAccount = accountService.GetCurrentAccount();
        if (!IsAccountOwnBrand(currentAccount, currentAccount.Brand))
            throw new BadRequestException("Account doesn't manage any brand");
        var brand = await GetBrandById(currentAccount.BrandId!.Value);
        var oldBannerId = brand.BannerId;
        var newLogo = await blobService.UploadImage(imageDto, nameof(Brand), nameof(Brand.Banner));
        brand.LogoId = newLogo.Id;
        unitOfWork.Brands.Update(brand);
        if (await unitOfWork.CompleteAsync() > 0 && oldBannerId.HasValue)
            await blobService.DeleteImageInFilesystem(oldBannerId.Value);
    }

    //TODO [Dat]: If truly delete, also remove Logo, Banner in filesystem
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
        // TODO [Duy]: implement this
        // return brand.BrandManagerId != null;
        throw new NotImplementedException();
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

    private static bool IsAccountOwnBrand(Account account, Brand? brand)
    {
        return brand != null
            && (
                account.HasRole(RoleEnum.Admin)
                || (account.HasRole(RoleEnum.BrandManager) && account.Brand!.Id == brand.Id)
            );
    }
}
