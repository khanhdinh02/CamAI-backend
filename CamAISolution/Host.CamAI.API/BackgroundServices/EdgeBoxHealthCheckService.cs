using Core.Domain;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService(
    IAppLogging<EdgeBoxHealthCheckService> logger,
    IServiceProvider provider
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
            while(true)
            {
                var edgeBoxInstallsPagination = await uow.GetRepository<EdgeBoxInstall>()
                    .GetAsync(
                        expression: e =>
                            e.EdgeBoxInstallStatus == EdgeBoxInstallStatus.Healthy
                            && e.EdgeBox.EdgeBoxStatus == EdgeBoxStatus.Active,
                        includeProperties: [nameof(EdgeBoxActivity.EdgeBox)],
                        pageIndex: pageIndex,
                        pageSize: pageSize
                    );
                await HandleEdgeBoxInstallHealthcheck(edgeBoxInstallsPagination.Values, stoppingToken, uow);
                if(pageIndex * edgeBoxInstallsPagination.PageSize + edgeBoxInstallsPagination.Values.Count >= edgeBoxInstallsPagination.TotalCount)
                    break;
                else
                    pageIndex++;
            }
            await Task.Delay(
                TimeSpan.FromSeconds(
                    scope.ServiceProvider.GetRequiredService<IConfiguration>().GetRequiredSection("HealthCheckConfiguration").Get<HealthCheckConfiguration>()!.EdgeBoxHealthCheckDelay
                ),
                stoppingToken
            );
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
        var failedEdgeBoxDic = failedHealthCheckEdgeBoxes.ToDictionary(
            s => s.Id,
            s => new Tuple<EdgeBoxInstall, int>(s, 0)
        );
        // TODO [Dat]: Use HealthCheckConfiguration instead
        var maxNumberOfRetry = 5;
        while (failedEdgeBoxDic.Any() && !cancellation.IsCancellationRequested)
        {
            var startOperationTime = new TimeSpan(DateTime.Now.Ticks);
            foreach (var edgeBoxInstall in failedHealthCheckEdgeBoxes)
            {
                var currentNumberOfRetry = failedEdgeBoxDic[edgeBoxInstall.Id].Item2;
                using var scope = provider.CreateScope();
                var edgeBoxService = scope.ServiceProvider.GetRequiredService<IEdgeBoxService>();
                if (currentNumberOfRetry > maxNumberOfRetry)
                {
                    failedEdgeBoxDic.Remove(edgeBoxInstall.Id);
                    await edgeBoxService.UpdateStatus(edgeBoxInstall.EdgeBoxId, EdgeBoxStatus.Broken);
                    continue;
                }
                try
                {
                    var res = await SendRequest($"{edgeBoxInstall.IpAddress}/api/test/{edgeBoxInstall.Id}");
                    if (res.IsSuccessStatusCode)
                    {
                        failedEdgeBoxDic.Remove(edgeBoxInstall.Id);
                        await edgeBoxService.UpdateStatus(edgeBoxInstall.EdgeBoxId, EdgeBoxStatus.Broken);
                    }
                    else
                        failedEdgeBoxDic[edgeBoxInstall.Id] = new Tuple<EdgeBoxInstall, int>(
                            edgeBoxInstall,
                            failedEdgeBoxDic[edgeBoxInstall.Id].Item2 + 1
                        );
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                    failedEdgeBoxDic[edgeBoxInstall.Id] = new Tuple<EdgeBoxInstall, int>(
                        edgeBoxInstall,
                        failedEdgeBoxDic[edgeBoxInstall.Id].Item2 + 1
                    );
                }
            }
            var endOperationTime = new TimeSpan(DateTime.Now.Ticks);
            var durration = endOperationTime.Subtract(startOperationTime);

            // TODO [Dat]: Consider to use HealthCheckConfiguration to get time to delay
            if (durration.Minutes < 5)
                await Task.Delay(TimeSpan.FromMinutes(5 - durration.Minutes), cancellation);
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
