using System.Collections.Concurrent;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Caching.Memory;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService(
    IAppLogging<EdgeBoxHealthCheckService> logger,
    IMemoryCache cache,
    IServiceProvider provider,
    HealthCheckConfiguration healthCheckConfiguration
) : BackgroundService
{
    private const int PageSize = 100;
    private static readonly ConcurrentBag<Guid> EdgeBoxIdThatResponse = [];
    private bool firstRun = true;
    private IUnitOfWork? uow;
    private readonly Mutex cacheMutex = new();

    public static void ReceivedEdgeBoxHealthResponse(Guid edgeBoxId) => EdgeBoxIdThatResponse?.Add(edgeBoxId);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // init service
                var scope = provider.CreateScope();

                // update edge box install that did not send the health check response back
                await UpdateEdgeBoxInstallStatusFromLastCheck(scope);

                // send health check message to bus
                await SendNextHealthCheckRequestMessage(scope, stoppingToken);

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
        // if this is first run, we won't update the status of eb install
        if (firstRun)
        {
            firstRun = false;
            return;
        }

        var responses = EdgeBoxIdThatResponse.ToList();
        EdgeBoxIdThatResponse.Clear();

        uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var edgeBoxInstallService = scope.ServiceProvider.GetRequiredService<IEdgeBoxInstallService>();

        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
        await jwtService.SetCurrentUserToSystemHandler();

        var edgeBoxInstallsPagination = new PaginationResult<EdgeBoxInstall> { Values = [new EdgeBoxInstall()] };
        var pageIndex = 0;
        while (!edgeBoxInstallsPagination.IsValuesEmpty)
        {
            edgeBoxInstallsPagination = await GetWorkingEdgeBoxInstall(pageIndex, responses);

            foreach (var edgeBoxInstall in edgeBoxInstallsPagination.Values)
            {
                await edgeBoxInstallService.UpdateStatus(edgeBoxInstall, EdgeBoxInstallStatus.Unhealthy);
                await notificationService.CreateNotification(await CreateNotification(edgeBoxInstall));
            }

            pageIndex++;
        }
    }

    private async Task<PaginationResult<EdgeBoxInstall>> GetWorkingEdgeBoxInstall(int pageIndex, List<Guid> responses)
    {
        return await uow!
            .GetRepository<EdgeBoxInstall>()
            .GetAsync(
                x => x.EdgeBoxInstallStatus == EdgeBoxInstallStatus.Working && !responses.Contains(x.EdgeBoxId),
                includeProperties: [nameof(EdgeBoxInstall.Shop)],
                pageIndex: pageIndex,
                pageSize: PageSize
            );
    }

    private static async Task SendNextHealthCheckRequestMessage(IServiceScope scope, CancellationToken stoppingToken)
    {
        var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        await bus.Publish(new HealthCheckRequestMessage(), stoppingToken);
    }

    private async Task<CreateNotificationDto> CreateNotification(EdgeBoxInstall edgeBoxInstall)
    {
        var sentTo = new List<Guid> { await GetAdminAccount() };
        if (edgeBoxInstall.Shop.ShopManagerId.HasValue)
            sentTo.Add(edgeBoxInstall.Shop.ShopManagerId.Value);

        var dto = new CreateNotificationDto
        {
            Title = "Edge box failed",
            Content = $"Edge box install status has been changed to {EdgeBoxInstallStatus.Unhealthy}",
            Priority = NotificationPriority.Urgent,
            Type = NotificationType.EdgeBoxUnhealthy,
            RelatedEntityId = edgeBoxInstall.Id,
            SentToId = sentTo
        };
        return dto;
    }

    private async Task<Guid> GetAdminAccount()
    {
        cacheMutex.WaitOne();
        var adminAccount = await cache.GetOrCreateAsync(
            "AdminAccounts",
            async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return (await uow!.Accounts.GetAsync(expression: a => a.Role == Role.Admin, takeAll: true)).Values[0];
            }
        );
        cacheMutex.ReleaseMutex();

        return adminAccount!.Id;
    }
}
