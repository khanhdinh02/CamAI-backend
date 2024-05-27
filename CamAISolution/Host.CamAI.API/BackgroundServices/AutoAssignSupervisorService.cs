using Core.Application.Implements;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Utilities;

namespace Host.CamAI.API.BackgroundServices;

public class AutoAssignSupervisorService(IServiceProvider provider) : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromMinutes(5);

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
            await Parallel.ForEachAsync(shops, stoppingToken, async (shop, _) => await AssignSupervisor(shop));
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async ValueTask AssignSupervisor(Shop shop)
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

        if (ShopService.IsShopOpeningAtTime(shop, currentTime))
        {
            // If there is an assignment after the shop opened, do nothing
            if (latestAsm != null && lastOpenTime <= latestAsm.StartTime)
                return;
            await unitOfWork.SupervisorAssignments.AddAsync(
                new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    StartTime = lastOpenTime,
                    EndTime = ShopService.GetNextCloseTime(shop),
                    SupervisorId = shop.ShopManagerId
                }
            );
        }
        else
        {
            var nextOpenTime = lastOpenTime.AddDays(1);
            if (latestAsm != null && nextOpenTime <= latestAsm.StartTime)
                return;
            await unitOfWork.SupervisorAssignments.AddAsync(
                new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    StartTime = nextOpenTime,
                    EndTime = ShopService.GetNextCloseTime(shop),
                    SupervisorId = shop.ShopManagerId
                }
            );
        }

        await unitOfWork.CompleteAsync();
    }
}
