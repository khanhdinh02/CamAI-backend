using Core.Domain.Models.Consumers;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

// TODO [Duy]: replace template with machine name
[Consumer("{Environment.MachineName}", ConsumerConstant.HumanCount)]
public class ClassifierConsumer : IConsumer<ClassifierModel>
{
    public async Task Consume(ConsumeContext<ClassifierModel> context)
    {
        // TODO [Duy]: get data to from context
        // TODO [Duy]: save data to file
        // TODO [Duy]: update data to some real-time platform

        throw new NotImplementedException();
    }
}
