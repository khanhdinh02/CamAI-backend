using Infrastructure.MessageQueue;
using static Host.CamAI.API.Consumers.ConsumerConstant;

namespace Host.CamAI.API.Models.Messages;

[Publisher(TestExchange)]
public class TestMessage
{
    public string RoutingKey { get; set; }
}
