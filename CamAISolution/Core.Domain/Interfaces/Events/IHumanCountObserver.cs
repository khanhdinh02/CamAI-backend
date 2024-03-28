using Core.Domain.Models.Consumers;

namespace Core.Domain.Interfaces.Events;

public interface IHumanCountObserver
{
    Guid ShopId { get; }
    Task ReceiveData(HumanCountModel model);
}
