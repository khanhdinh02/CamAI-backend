using Core.Application.Events;
using Core.Domain.Interfaces.Events;
using Core.Domain.Models.Consumers;

namespace Core.Application.Models;

public class HumanCountBuffer : CircularBuffer<HumanCountModel>, IHumanCountObserver
{
    public HumanCountBuffer(HumanCountSubject subject, Guid shopId)
        : base(10)
    {
        ShopId = shopId;
        subject.Attach(this);
    }

    public Guid ShopId { get; }

    public void ReceiveData(HumanCountModel model) => Write(model);

    // TODO [Duy]: Detach and dispose
}
