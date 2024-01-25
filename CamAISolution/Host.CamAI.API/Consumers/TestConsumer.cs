using Host.CamAI.API.Models.Messages;
using Infrastructure.MessageQueue;
using MassTransit;
using RabbitMQ.Client;
using static Host.CamAI.API.Consumers.ConsumerConstant;

namespace Host.CamAI.API.Consumers;

[Consumer(TestQueue, TestExchange, "routingKey2", ExchangeType.Direct)]
public class TestConsumer : IConsumer<TestMessage>
{
    public async Task Consume(ConsumeContext<TestMessage> context)
    {
        throw new NotImplementedException();
    }
}
