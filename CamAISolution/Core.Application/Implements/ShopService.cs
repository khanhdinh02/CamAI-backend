using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Application.Specifications.Shops.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.DTO.Accounts;
using Core.Domain.Models.DTO.Brands;
using Core.Domain.Models.DTO.Shops;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class ShopService(
    IUnitOfWork unitOfWork,
    IAppLogging<ShopService> logger,
    IBaseMapping mapping,
    IAccountService accountService,
    IBrandService brandService) : IShopService
{
    public async Task<Shop> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        await IsValidShopDto(shopDto);
        var shop = mapping.Map<CreateOrUpdateShopDto, Shop>(shopDto);
        shop.ShopStatusId = ShopStatusEnum.Active;
        shop = await unitOfWork.Shops.AddAsync(shop);
        await unitOfWork.CompleteAsync();
        return shop;
    }

    public Task DeleteShop(Guid id)
    {
        logger.Info($"{nameof(DeleteShop)} was not Implemented");
        throw new ServiceUnavailableException("");
    }

    public async Task<Shop> GetShopById(Guid id)
    {
        var foundShop = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id));
        if (foundShop.Values.Count == 0)
            throw new NotFoundException(typeof(Shop), id);
        return foundShop.Values[0];
    }

    public async Task<PaginationResult<Shop>> GetShops(SearchShopRequest searchRequest)
    {
        if (searchRequest.StatusId.HasValue && searchRequest.StatusId.Value == ShopStatusEnum.Inactive && !await IsAdmin())
            return new PaginationResult<Shop>();
        var shops = await unitOfWork.Shops.GetAsync(new SearchShopSpec(searchRequest));
        return shops;
    }

    public async Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto)
    {
        var foundShops = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id, false));
        if (foundShops.Values.Count == 0)
            throw new NotFoundException(typeof(Shop), id);
        var foundShop = foundShops.Values[0];
        if (foundShop.ShopStatusId == ShopStatusEnum.Inactive && !await IsAdmin())
            throw new BadRequestException("Cannot modified inactive shop");
        await IsValidShopDto(shopDto);
        mapping.Map(shopDto, foundShop);
        await unitOfWork.CompleteAsync();
        return await GetShopById(id);
    }

    public async Task<Shop> UpdateStatus(Guid shopId, int shopStatusId)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(shopId);
        if (foundShop == null)
            throw new NotFoundException(typeof(Shop), shopId);
        if (foundShop.ShopStatusId == ShopStatusEnum.Inactive && !await IsAdmin())
            throw new BadRequestException($"Cannot update inactive shop");
        foundShop.ShopStatusId = shopStatusId;
        await unitOfWork.CompleteAsync();
        return await GetShopById(shopId);
    }

    private async Task<bool> IsAdmin()
    {
        var account = await accountService.GetCurrentAccount();
        return account.Roles.Any(r => r.Id == RoleEnum.Admin);
    }

    private async Task IsValidShopDto(CreateOrUpdateShopDto shopDto)
    {
        var isFoundWard = await unitOfWork.Wards.IsExisted(shopDto.WardId);
        if (!isFoundWard)
            throw new NotFoundException(typeof(Ward), shopDto.WardId);
        var foundBrand = await brandService.GetBrandById(shopDto.BrandId);
        if(foundBrand.BrandStatusId == BrandStatusEnum.Inactive)
        {
            logger.Error($"Found Brand is {nameof(BrandStatusEnum.Inactive)}. Cannot updated");
            throw new NotFoundException(typeof(Brand), shopDto.BrandId);
        }
    }
}
