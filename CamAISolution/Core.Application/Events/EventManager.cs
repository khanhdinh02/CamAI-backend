using Core.Domain.Entities;

namespace Core.Application.Events;

public class EventManager
{
    public delegate void ActivateEdgeBox(Guid edgeBoxId);

    public delegate void BrandChanged(Brand brand);

    public delegate void ShopChanged(Shop shop);

    public event ActivateEdgeBox? ActivateEdgeBoxEvent;

    public void NotifyActivatedEdgeBox(Guid edgeBoxId)
    {
        ActivateEdgeBoxEvent?.Invoke(edgeBoxId);
    }

    public event BrandChanged? BrandChangedEvent;

    public void NotifyBrandChanged(Brand brand)
    {
        BrandChangedEvent?.Invoke(brand);
    }

    public event ShopChanged? ShopChangedEvent;

    public void NotifyShopChanged(Shop shop)
    {
        ShopChangedEvent?.Invoke(shop);
    }
}