using Infrastructure.MessageQueue;
using static Infrastructure.MessageQueue.ConsumerConstant;

namespace Host.CamAI.API.Models.Messages;

[Publisher(TestExchange)]
public class TestMessage
{
    public string RoutingKey { get; set; }
}
