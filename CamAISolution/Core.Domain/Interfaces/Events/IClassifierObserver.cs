using Core.Domain.Models.Consumers;

namespace Core.Domain.Interfaces.Events;

public interface IClassifierObserver
{
    Guid ShopId { get; }
    void ReceiveData(ClassifierModel model);
}
