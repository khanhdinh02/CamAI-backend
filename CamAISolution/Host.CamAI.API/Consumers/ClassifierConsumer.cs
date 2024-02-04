using Core.Application.Events;
using Core.Domain.Models.Consumers;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}", ConsumerConstant.HumanCount)]
public class ClassifierConsumer(ClassifierSubject subject) : IConsumer<ClassifierModel>
{
    public Task Consume(ConsumeContext<ClassifierModel> context)
    {
        // TODO [Duy]: save data to file

        subject.Notify(context.Message);
        return Task.CompletedTask;
    }
}
