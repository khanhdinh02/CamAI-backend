using Core.Application.Events;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;
using Serilog;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}", ConsumerConstant.HumanCount)]
public class HumanCountConsumer(HumanCountSubject subject) : IConsumer<HumanCountModelContract>
{
    public Task Consume(ConsumeContext<HumanCountModelContract> context)
    {
        subject.Notify(context.Message.ToHumanCountModel());
        return Task.CompletedTask;
    }
}
