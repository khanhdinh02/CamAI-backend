using Core.Application.Events;
using Core.Domain.Entities;
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
            // TODO: define the routing key
            RoutingKey = "brand1.shop1"
        };
        SendMessage(updateMessage);
    }

    private void SyncShop(Shop shop)
    {
        // TODO: define the routing key
        var updateMessage = new ShopUpdateMessage
        {
            Name = shop.Name,
            // TODO: change address line to combine both province and ward
            Address = shop.AddressLine,
            Phone = shop.Phone,
            RoutingKey = ""
        };
        SendMessage(updateMessage);
    }
}
