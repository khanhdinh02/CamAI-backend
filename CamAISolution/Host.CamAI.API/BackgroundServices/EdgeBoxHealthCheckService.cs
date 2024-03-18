using System.Net.Http.Headers;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService(
    IAppLogging<EdgeBoxHealthCheckService> logger,
    IMemoryCache cache,
    IServiceProvider provider,
    HealthCheckConfiguration healthCheckConfiguration
) : BackgroundService
{
    private readonly HttpClient httpClient = CreateHttpClient();
    private IEdgeBoxInstallService? edgeBoxInstallService;
    private INotificationService? notificationService;
    private IUnitOfWork? uow;
    private readonly Mutex mutex = new();

    private static HttpClient CreateHttpClient()
    {
        // disable ssl
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // init service
            var scope = provider.CreateScope();

            var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
            await jwtService.SetCurrentUserToSystemHandler();

            uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            edgeBoxInstallService = scope.ServiceProvider.GetRequiredService<IEdgeBoxInstallService>();
            notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            // fetch 100 edge box install each
            const int pageSize = 100;
            var edgeBoxInstallsPagination = new PaginationResult<EdgeBoxInstall> { Values = [new EdgeBoxInstall()] };
            var pageIndex = 0;
            while (!edgeBoxInstallsPagination.IsValuesEmpty)
            {
                edgeBoxInstallsPagination = await uow.GetRepository<EdgeBoxInstall>()
                    .GetAsync(
                        x =>
                            x.EdgeBoxInstallStatus == EdgeBoxInstallStatus.Working
                            || x.EdgeBoxInstallStatus == EdgeBoxInstallStatus.Unhealthy,
                        includeProperties: [nameof(EdgeBoxInstall.Shop)],
                        pageIndex: pageIndex,
                        pageSize: pageSize
                    );

                // await CheckEdgeBoxInstallHealth(edgeBoxInstallsPagination.Values, stoppingToken);
                pageIndex++;
            }

            await Task.Delay(TimeSpan.FromSeconds(healthCheckConfiguration.EdgeBoxHealthCheckDelay), stoppingToken);
        }
    }

    private async Task CheckEdgeBoxInstallHealth(IList<EdgeBoxInstall> edgeBoxInstalls, CancellationToken cancellation)
    {
        var notificationDto = new List<CreateNotificationDto>();
        await Parallel.ForEachAsync(
            edgeBoxInstalls,
            new ParallelOptions { MaxDegreeOfParallelism = 8 },
            async (edgeBoxInstall, cancel) =>
            {
                for (var numOfRetry = 0; numOfRetry < healthCheckConfiguration.MaxNumberOfRetry; numOfRetry++)
                {
                    try
                    {
                        // call edge box endpoint
                        var response = await SendRequest(GetEdgeBoxLink(edgeBoxInstall));
                        if (response.IsSuccessStatusCode)
                        {
                            await LockUpdateStatus(edgeBoxInstall, EdgeBoxInstallStatus.Working);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message, ex);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(healthCheckConfiguration.RetryDelay), cancel);
                }

                // failed after max retry
                // this will update status and send notification
                // TODO: only update notified if update status
                await LockUpdateStatus(edgeBoxInstall, EdgeBoxInstallStatus.Unhealthy);

                notificationDto.Add(await CreateNotification(edgeBoxInstall));
            }
        );
        foreach (var dto in notificationDto)
            await notificationService!.CreateNotification(dto, true);
    }

    private async Task LockUpdateStatus(EdgeBoxInstall edgeBoxInstall, EdgeBoxInstallStatus status)
    {
        mutex.WaitOne();
        await edgeBoxInstallService!.UpdateStatus(edgeBoxInstall, status);
        mutex.ReleaseMutex();
    }

    private static string GetEdgeBoxLink(EdgeBoxInstall edgeBoxInstall)
    {
        return $"http://{edgeBoxInstall.IpAddress}:{edgeBoxInstall.Port}/api/test/{edgeBoxInstall.Id}";
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
            NotificationType = NotificationType.Urgent,
            SentToId = sentTo
        };
        return dto;
    }

    private async Task<Guid> GetAdminAccount()
    {
        var adminAccount = await cache.GetOrCreateAsync(
            "AdminAccounts",
            async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return (await uow!.Accounts.GetAsync(expression: a => a.Role == Role.Admin, takeAll: true)).Values[0];
            }
        );

        return adminAccount!.Id;
    }

    private Task<HttpResponseMessage> SendRequest(string uri) => httpClient.GetAsync(new Uri(uri));
}
