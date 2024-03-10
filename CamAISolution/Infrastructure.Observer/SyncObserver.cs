using Core.Application.Events;
using Core.Domain.Entities;
using Core.Domain.Utilities;
using Infrastructure.Observer.Messages;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Observer;

public class SyncObserver(EventManager eventManager, IServiceProvider provider)
{
    public void RegisterEvent()
    {
        eventManager.BrandChangedEvent += SyncBrand;
        eventManager.ShopChangedEvent += SyncShop;
    }

    private void SendMessage<T>(T message)
        where T : class
    {
        using var scope = provider.CreateScope();
        var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        bus.Publish(message).Wait();
    }

    public void SyncBrand(Brand brand) => SyncBrand(brand, $"{brand.Id}.*");

    public void SyncBrand(Brand brand, string? routingKey = null)
    {
        routingKey ??= $"{brand.Id}.*";
        var updateMessage = new BrandUpdateMessage
        {
            Name = brand.Name,
            Email = brand.Email,
            Phone = brand.Phone,
            RoutingKey = routingKey
        };
        SendMessage(updateMessage);
    }

    public void SyncShop(Shop shop) => SyncShop(shop, $"{shop.BrandId}.{shop.Id}");

    public void SyncShop(Shop shop, string? routingKey = null)
    {
        routingKey ??= $"{shop.BrandId}.{shop.Id}";
        var updateMessage = new ShopUpdateMessage
        {
            Name = shop.Name,
            Address = ProvinceHelper.GetFullAddress(shop.AddressLine, shop.Ward),
            Phone = shop.Phone,
            RoutingKey = routingKey
        };
        SendMessage(updateMessage);
    }
}
