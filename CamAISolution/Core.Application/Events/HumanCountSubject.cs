using Core.Domain.Interfaces.Events;
using Core.Domain.Models.Consumers;

namespace Core.Application.Events;

public class HumanCountSubject
{
    private readonly List<IHumanCountObserver> observers = [];

    public void Attach(IHumanCountObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IHumanCountObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(HumanCountModel model)
    {
        // if shopId is Guid.Empty, it will receive all events
        foreach (var o in observers.Where(x => x.ShopId == model.ShopId || x.ShopId == Guid.Empty))
            o.ReceiveData(model);
    }
}
