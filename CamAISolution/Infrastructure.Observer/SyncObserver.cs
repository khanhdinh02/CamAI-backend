using Core.Application.Events;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Utilities;
using Infrastructure.Observer.Messages;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Observer;

public class SyncObserver(EventManager eventManager, IServiceProvider provider)
{
    public void RegisterEvent()
    {
        eventManager.BrandChangedEvent += brand => SyncBrand(brand);
        eventManager.ShopChangedEvent += shop => SyncShop(shop);
    }

    private async Task SendMessage<T>(T message)
        where T : class
    {
        using var scope = provider.CreateScope();
        var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        await bus.Publish(message);
    }

    public async Task SyncBrand(Brand brand, string? routingKey = null)
    {
        routingKey ??= $"{brand.Id:N}.*";
        var updateMessage = new BrandUpdateMessage
        {
            Id = brand.Id,
            Name = brand.Name,
            Email = brand.Email,
            Phone = brand.Phone,
            RoutingKey = routingKey
        };
        await SendMessage(updateMessage);
    }

    public async Task SyncShop(Shop shop, string? routingKey = null)
    {
        routingKey ??= $"{shop.BrandId:N}.{shop.Id:N}";
        var updateMessage = new ShopUpdateMessage
        {
            Id = shop.Id,
            Name = shop.Name,
            Address = ProvinceHelper.GetFullAddress(shop.AddressLine, shop.Ward),
            Phone = shop.Phone,
            OpenTime = shop.OpenTime,
            CloseTime = shop.CloseTime,
            RoutingKey = routingKey
        };
        await SendMessage(updateMessage);
    }

    public async Task SyncCamera(IList<Camera> cameras, string routingKey)
    {
        using var scope = provider.CreateScope();
        var mapper = scope.ServiceProvider.GetRequiredService<IBaseMapping>();
        var updateMessage = new CameraUpdateMessage
        {
            RoutingKey = routingKey,
            Cameras = cameras.Select(mapper.Map<Camera, EdgeBoxCameraDto>).ToList()
        };
        await SendMessage(updateMessage);
    }
}
