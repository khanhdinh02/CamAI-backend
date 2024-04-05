using Core.Application.Events;
using Core.Domain;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_HumanCount", ConsumerConstant.HumanCount)]
public class HumanCountConsumer(HumanCountSubject subject, IAppLogging<HumanCountConsumer> logger)
    : IConsumer<HumanCountMessage>
{
    public Task Consume(ConsumeContext<HumanCountMessage> context)
    {
        logger.Info($"Receive new human count data for shop {context.Message.ShopId}");
        subject.Notify(context.Message.ToHumanCountModel());
        return Task.CompletedTask;
    }
}
