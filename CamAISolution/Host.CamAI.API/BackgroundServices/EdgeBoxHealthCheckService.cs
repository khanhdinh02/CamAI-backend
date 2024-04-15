using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.Configurations;
using Core.Domain.Services;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService(
    IAppLogging<EdgeBoxHealthCheckService> logger,
    IServiceProvider provider,
    HealthCheckConfiguration healthCheckConfiguration
) : BackgroundService
{
    private ICacheService? cacheService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // init service
                var scope = provider.CreateScope();

                await UpdateEdgeBoxInstallStatusFromLastCheck(scope);

                await Task.Delay(TimeSpan.FromSeconds(healthCheckConfiguration.EdgeBoxHealthCheckDelay), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
        }
    }

    private async Task UpdateEdgeBoxInstallStatusFromLastCheck(IServiceScope scope)
    {
        var edgeBoxInstallService = scope.ServiceProvider.GetRequiredService<IEdgeBoxInstallService>();
        await edgeBoxInstallService.UpdateEdgeBoxHealthByLastSeen(
            TimeSpan.FromSeconds(healthCheckConfiguration.UnhealthyElapsedTime)
        );

        // init service
        cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // init paging result
        PaginationResult<EdgeBoxInstall> edgeBoxInstallsPagination;
        var searchRequest = new SearchEdgeBoxInstallRequest
        {
            EdgeBoxLocation = EdgeBoxLocation.Occupied,
            EdgeBoxInstallStatus = EdgeBoxInstallStatus.Unhealthy,
            EndLastSeen = DateTime.Now - TimeSpan.FromSeconds(healthCheckConfiguration.UnhealthyNotifyTime),
            PageIndex = 0,
            Size = 100
        };
        do
        {
            edgeBoxInstallsPagination = await edgeBoxInstallService.GetEdgeBoxInstall(searchRequest);
            foreach (var edgeBoxInstall in edgeBoxInstallsPagination.Values)
                await notificationService.CreateNotification(await CreateNotification(edgeBoxInstall));
            searchRequest.PageIndex += 1;
        } while (!edgeBoxInstallsPagination.IsValuesEmpty);
    }

    private async Task<CreateNotificationDto> CreateNotification(EdgeBoxInstall edgeBoxInstall)
    {
        var sentTo = new List<Guid> { await cacheService!.GetAdminAccount() };
        if (edgeBoxInstall.Shop.ShopManagerId.HasValue)
            sentTo.Add(edgeBoxInstall.Shop.ShopManagerId.Value);

        var dto = new CreateNotificationDto
        {
            Title = "Edge box is unhealthy failed",
            Content = $"Edge box does not response. Status changed to {EdgeBoxInstallStatus.Unhealthy}",
            Priority = NotificationPriority.Urgent,
            Type = NotificationType.EdgeBoxUnhealthy,
            RelatedEntityId = edgeBoxInstall.Id,
            SentToId = sentTo
        };
        return dto;
    }
}
