using Core.Application.Events;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_HumanCount", ConsumerConstant.HumanCount)]
public class HumanCountConsumer(HumanCountSubject subject) : IConsumer<HumanCountMessage>
{
    public Task Consume(ConsumeContext<HumanCountMessage> context)
    {
        subject.Notify(context.Message.ToHumanCountModel());
        return Task.CompletedTask;
    }
}
