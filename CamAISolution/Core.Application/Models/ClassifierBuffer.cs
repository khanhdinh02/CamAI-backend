using Core.Application.Events;
using Core.Domain.Models.Consumers;

namespace Core.Application.Models;

public class ClassifierBuffer : CircularBuffer<ClassifierModel>, IClassifierObserver
{
    public ClassifierBuffer(ClassifierSubject subject, Guid shopId)
        : base(10)
    {
        ShopId = shopId;
        subject.Attach(this);
    }

    public Guid ShopId { get; }

    public void ReceiveData(ClassifierModel model) => Write(model);

    // TODO [Duy]: Detach and dispose
}
