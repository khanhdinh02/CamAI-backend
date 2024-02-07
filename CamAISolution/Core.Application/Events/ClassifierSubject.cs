using Core.Domain.Interfaces.Events;
using Core.Domain.Models.Consumers;

namespace Core.Application.Events;

public class ClassifierSubject
{
    private readonly List<IClassifierObserver> observers = [];

    public void Attach(IClassifierObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IClassifierObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(ClassifierModel model)
    {
        // if shopId is Guid.Empty, it will receive all events
        foreach (var o in observers.Where(x => x.ShopId == model.ShopId || x.ShopId == Guid.Empty))
            o.ReceiveData(model);
    }
}
