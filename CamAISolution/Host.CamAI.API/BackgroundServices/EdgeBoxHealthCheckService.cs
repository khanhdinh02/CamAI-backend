using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService(
    IAppLogging<EdgeBoxHealthCheckService> logger,
    IServiceProvider provider,
    HealthCheckConfiguration healthCheckConfiguration
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = provider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            int pageIndex = 0;
            int pageSize = 100;
            while (true)
            {
                var edgeBoxInstallsPagination = await uow.GetRepository<EdgeBoxInstall>()
                    .GetAsync(
                        expression: e =>
                        e.EdgeBox.EdgeBoxStatus == EdgeBoxStatus.Active,
                        includeProperties: [nameof(EdgeBoxActivity.EdgeBox)],
                        pageIndex: pageIndex,
                        pageSize: pageSize
                    );
                await HandleEdgeBoxInstallHealthcheck(edgeBoxInstallsPagination.Values, stoppingToken, uow);
                if (pageIndex * edgeBoxInstallsPagination.PageSize + edgeBoxInstallsPagination.Values.Count >= edgeBoxInstallsPagination.TotalCount)
                    break;
                else
                    pageIndex++;
            }
            await Task.Delay(TimeSpan.FromSeconds(healthCheckConfiguration.EdgeBoxHealthCheckDelay), stoppingToken);
        }
    }

    private async Task HandleEdgeBoxInstallHealthcheck(IEnumerable<EdgeBoxInstall> edgeBoxInstalls, CancellationToken cancellation, IUnitOfWork uow)
    {
        HashSet<EdgeBoxInstall> failedEdgeBoxInstall = new();
        await uow.BeginTransaction();
        foreach (var edgeBoxInstall in edgeBoxInstalls)
        {
            try
            {
                var res = await SendRequest($"{edgeBoxInstall.IpAddress}:{edgeBoxInstall.Port}/api/test/{edgeBoxInstall.Id}");
                if (res.IsSuccessStatusCode)
                {
                    if (edgeBoxInstall.EdgeBox.EdgeBoxStatus != EdgeBoxStatus.Active)
                    {
                        edgeBoxInstall.EdgeBoxInstallStatus = EdgeBoxInstallStatus.Healthy;
                        await uow.CompleteAsync();
                    }
                }
                else
                    failedEdgeBoxInstall.Add(edgeBoxInstall);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                failedEdgeBoxInstall.Add(edgeBoxInstall);
            }
        }
        await HealthCheckAllFailedEdgeBox(failedEdgeBoxInstall, cancellation);
        try
        {
            await uow.CommitTransaction();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            await uow.RollBack();
        }
    }

    private async Task HealthCheckAllFailedEdgeBox(
        IEnumerable<EdgeBoxInstall> failedHealthCheckEdgeBoxes,
        CancellationToken cancellation
    )
    {
        var failedEdgeBoxDic = failedHealthCheckEdgeBoxes.ToDictionary(s => s.Id, s => 0);
        while (failedEdgeBoxDic.Any() && !cancellation.IsCancellationRequested)
        {
            var startOperationTime = new TimeSpan(DateTime.Now.Ticks);
            foreach (var edgeBoxInstall in failedHealthCheckEdgeBoxes)
            {
                var currentNumberOfRetry = failedEdgeBoxDic[edgeBoxInstall.Id];
                using var scope = provider.CreateScope();
                var edgeBoxInstallService = scope.ServiceProvider.GetRequiredService<IEdgeBoxInstallService>();
                if (currentNumberOfRetry > healthCheckConfiguration.MaxNumberOfRetry)
                {
                    failedEdgeBoxDic.Remove(edgeBoxInstall.Id);
                    await edgeBoxInstallService.UpdateStatus(edgeBoxInstall.Id, EdgeBoxInstallStatus.Unhealthy, edgeBoxInstall);
                    continue;
                }
                try
                {
                    var res = await SendRequest($"{edgeBoxInstall.IpAddress}:{edgeBoxInstall.Port}/api/test/{edgeBoxInstall.Id}");
                    if (res.IsSuccessStatusCode)
                    {
                        failedEdgeBoxDic.Remove(edgeBoxInstall.Id);
                        await edgeBoxInstallService.UpdateStatus(edgeBoxInstall.Id, EdgeBoxInstallStatus.Unhealthy, edgeBoxInstall);
                    }
                    else
                        failedEdgeBoxDic[edgeBoxInstall.Id] = ++failedEdgeBoxDic[edgeBoxInstall.Id];
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                    failedEdgeBoxDic[edgeBoxInstall.Id] = ++failedEdgeBoxDic[edgeBoxInstall.Id];
                }
            }
            var endOperationTime = new TimeSpan(DateTime.Now.Ticks);
            var durration = endOperationTime.Subtract(startOperationTime);

            if (durration.Seconds < 5 * 60)
                await Task.Delay(TimeSpan.FromMinutes(healthCheckConfiguration.RetryDelay - durration.Seconds), cancellation);
        }
    }

    private Task<HttpResponseMessage> SendRequest(string uri)
    {
        var http = new HttpClient();
        http.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
        );
        return http.GetAsync(new Uri(uri));
    }
}
