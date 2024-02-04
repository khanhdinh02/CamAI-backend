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
        foreach (var o in observers.Where(x => x.ShopId == model.ShopId))
            o.ReceiveData(model);
    }
}
