using Core.Application.Events;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;
using Serilog;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}", ConsumerConstant.Classifier)]
public class ClassifierConsumer(ClassifierSubject subject) : IConsumer<ClassifierModelContract>
{
    public Task Consume(ConsumeContext<ClassifierModelContract> context)
    {
        // TODO [Duy]: save data to file

        Log.Information("Receive classifier message");
        subject.Notify(context.Message.ToClassifierModel());
        return Task.CompletedTask;
    }
}
