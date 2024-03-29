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
        eventManager.BrandChangedEvent += brand => SyncBrand(brand);
        eventManager.ShopChangedEvent += shop => SyncShop(shop);
    }

    private void SendMessage<T>(T message)
        where T : class
    {
        using var scope = provider.CreateScope();
        var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        bus.Publish(message).Wait();
    }

    public void SyncBrand(Brand brand, string? routingKey = null)
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
        SendMessage(updateMessage);
    }

    public void SyncShop(Shop shop, string? routingKey = null)
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
        SendMessage(updateMessage);
    }
}
