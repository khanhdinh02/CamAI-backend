using System.Collections.Concurrent;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopsController(IAppLogging<ShopsController> logger, IAccountService accountService, IServiceProvider serviceProvider, IShopService shopService, IBaseMapping baseMapping) : ControllerBase
{
    private static readonly ConcurrentDictionary<string, Task> tasks = new();

    /// <summary>
    /// Search Shop base on current account's roles.
    /// </summary>
    /// <param name="search"></param>
    /// <remarks>
    /// <para>
    ///     Admin can search every shops.
    /// </para>
    /// <para>
    ///     Brand manager can search every shops in his/her brand.
    /// </para>
    /// <para>
    ///     Shop manager can only see his/her shop
    /// </para>
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<ActionResult<PaginationResult<ShopDto>>> GetCurrentShop([FromQuery] ShopSearchRequest search)
    {
        var shops = await shopService.GetShops(search);
        return Ok(baseMapping.Map<Shop, ShopDto>(shops));
    }

    [HttpGet("{id}")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<ActionResult<ShopDto>> GetShopById(Guid id)
    {
        return Ok(baseMapping.Map<Shop, ShopDto>(await shopService.GetShopById(id)));
    }

    [HttpPost]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<ShopDto>> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        return Ok(baseMapping.Map<Shop, ShopDto>(await shopService.CreateShop(shopDto)));
    }

    [HttpPut("{id:guid}")]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<ShopDto>> UpdateShop(Guid id, [FromBody] CreateOrUpdateShopDto shopDto)
    {
        var updatedShop = await shopService.UpdateShop(id, shopDto);
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpPatch("{id:guid}/status/{shopStatus}")]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<ShopDto>> UpdateShopStatus(Guid id, ShopStatus shopStatus)
    {
        var updatedShop = await shopService.UpdateShopStatus(id, shopStatus);
        if (updatedShop.ShopStatus == ShopStatus.Inactive)
            return base.Ok();
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpDelete("{id:guid}")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<IActionResult> DeleteShop(Guid id)
    {
        await shopService.DeleteShop(id);
        return Accepted();
    }

    /// <summary>
    /// Get all shops that currently have edge box installing or not.
    /// </summary>
    /// <remarks>For role Admin</remarks>
    /// <param name="q"></param>
    /// <returns></returns>
    [HttpGet("installing")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<PaginationResult<ShopDto>>> GetShopsInstallingEdgeBox(bool q)
    {
        var shops = await shopService.GetShopsInstallingEdgeBox(q);
        return Ok(baseMapping.Map<Shop, ShopDto>(shops));
    }

    [HttpPost("upsert")]
    [AccessTokenGuard(Role.BrandManager)]
    public Task<IActionResult> UpsertShopAndManager(IFormFile file)
    {
        var actorId = accountService.GetCurrentAccount().Id;
        var id = Guid.NewGuid().ToString("N");
        var bulkTask = Task.Run(async () =>
        {

            try
            {
                var scope = serviceProvider.CreateScope();
                var scopeShopService = scope.ServiceProvider.GetRequiredService<IShopService>();
                var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await jwtService.SetCurrentUserToSystemHandler();
                await scopeShopService.UpsertShop(actorId, stream);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

        }).ContinueWith(_ => tasks.TryRemove(id, out _));
        tasks.TryAdd(id, bulkTask);
        var res = new BulkResponse()
        {
            TaskId = id,
            Message = $"Task is accepted"
        };
        return Task.FromResult<IActionResult>(Ok(res));
    }
}
