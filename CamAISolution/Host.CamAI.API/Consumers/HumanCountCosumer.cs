using Core.Domain.Models.Consumers;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

// TODO [Duy]: replace template with machine name
[Consumer("{Environment.MachineName}", ConsumerConstant.HumanCount)]
public class HumanCountCosumer : IConsumer<HumanCount>
{
    public async Task Consume(ConsumeContext<HumanCount> context)
    {
        // TODO [Duy]: get data to from context
        // TODO [Duy]: save data to file
        // TODO [Duy]: update data to some real-time platform

        throw new NotImplementedException();
    }
}
