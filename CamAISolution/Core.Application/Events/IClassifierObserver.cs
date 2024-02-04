using Core.Domain.Models.Consumers;

namespace Core.Application.Events;

public interface IClassifierObserver
{
    Guid ShopId { get; }
    void ReceiveData(ClassifierModel model);
}
