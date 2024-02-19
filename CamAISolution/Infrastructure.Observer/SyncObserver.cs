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
        bus.Publish(message);
    }

    private void SyncBrand(Brand brand)
    {
        var updateMessage = new BrandUpdateMessage
        {
            Name = brand.Name,
            Email = brand.Email,
            Phone = brand.Phone,
            RoutingKey = $"{brand.Id}.*"
        };
        SendMessage(updateMessage);
    }

    private void SyncShop(Shop shop)
    {
        var updateMessage = new ShopUpdateMessage
        {
            Name = shop.Name,
            Address = ProvinceHelper.GetFullAddress(shop.AddressLine, shop.Ward),
            Phone = shop.Phone,
            RoutingKey = $"{shop.BrandId}.{shop.Id}"
        };
        SendMessage(updateMessage);
    }
}
