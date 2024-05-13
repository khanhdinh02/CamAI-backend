using Core.Application.Implements;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Utilities;

namespace Host.CamAI.API.BackgroundServices;

public class AutoAssignHeadSupervisorService(IServiceProvider provider) : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromSeconds(60);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = provider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var timer = new PeriodicTimer(Period);
        do
        {
            var shops = (
                await unitOfWork.Shops.GetAsync(shop => shop.ShopStatus == ShopStatus.Active, takeAll: true)
            ).Values;
            await Parallel.ForEachAsync(shops, stoppingToken, (shop, _) => AssignHeadSupervisorService(shop));
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async ValueTask AssignHeadSupervisorService(Shop shop)
    {
        var scope = provider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var currentDateTime = DateTimeHelper.VNDateTime;
        var currentTime = TimeOnly.FromDateTime(currentDateTime);
        var lastOpenTime = ShopService.GetLastOpenTime(shop);
        var latestAsm = (
            await unitOfWork.SupervisorAssignments.GetAsync(
                asm => asm.ShopId == shop.Id,
                orderBy: o => o.OrderByDescending(x => x.StartTime),
                pageSize: 1
            )
        ).Values.FirstOrDefault();

        if (latestAsm?.HeadSupervisorId == null)
            return;

        if (ShopService.IsShopOpeningAtTime(shop, currentTime))
        {
            if (lastOpenTime <= latestAsm.StartTime)
                return;
            await unitOfWork.SupervisorAssignments.AddAsync(
                new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    HeadSupervisorId = latestAsm.HeadSupervisorId,
                    StartTime = currentDateTime,
                    EndTime = ShopService.GetNextCloseTime(shop)
                }
            );
        }
        else
        {
            var nextOpenTime = lastOpenTime.AddDays(1);
            if (nextOpenTime <= latestAsm.StartTime)
                return;
            await unitOfWork.SupervisorAssignments.AddAsync(
                new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    HeadSupervisorId = latestAsm.HeadSupervisorId,
                    StartTime = nextOpenTime,
                    EndTime = ShopService.GetNextCloseTime(shop)
                }
            );
        }

        await unitOfWork.CompleteAsync();
    }
}
