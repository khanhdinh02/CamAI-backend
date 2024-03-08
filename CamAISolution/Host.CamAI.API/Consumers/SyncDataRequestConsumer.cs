using Core.Domain.Repositories;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using Infrastructure.Observer;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}", ConsumerConstant.SyncDataRequest)]
public class SyncDataRequestConsumer(SyncObserver syncObserver, IUnitOfWork unitOfWork, IPublishEndpoint bus)
    : IConsumer<SyncDataRequest>
{
    public async Task Consume(ConsumeContext<SyncDataRequest> context)
    {
        // TODO [Duy]: Use ebinstall service after implemented
        var edgeBoxId = context.Message.EdgeBoxId;
        var ebInstall = (
            await unitOfWork.EdgeBoxInstalls.GetAsync(
                x => x.EdgeBoxId == edgeBoxId,
                includeProperties: ["Shop.Brand", "Shop.Ward.District.Province"]
            )
        ).Values[0];

        syncObserver.SyncBrand(ebInstall.Shop.Brand, edgeBoxId.ToString("N"));
        syncObserver.SyncShop(ebInstall.Shop, edgeBoxId.ToString("N"));
    }
}
