using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService(
    IAppLogging<EdgeBoxHealthCheckService> logger,
    IServiceProvider provider,
    IUnitOfWork uow
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = provider.CreateScope();
            var edgeBoxInstalls = (
                await uow.GetRepository<EdgeBoxInstall>()
                    .GetAsync(
                        expression: e =>
                            e.EdgeBoxInstallStatusId == EdgeBoxInstallStatusEnum.Valid
                            && e.EdgeBox.EdgeBoxStatusId == EdgeBoxStatusEnum.Active,
                        takeAll: true,
                        includeProperties: [nameof(EdgeBoxActivity.EdgeBox)]
                    )
            ).Values;
            HashSet<EdgeBoxInstall> failedEdgeBoxInstall = new();
            await uow.BeginTransaction();
            foreach (var edgeBoxInstall in edgeBoxInstalls)
            {
                int? newStatusId;
                try
                {
                    var res = await SendRequest($"{edgeBoxInstall.IpAddress}/api/{edgeBoxInstall.Id}");
                    newStatusId = res.IsSuccessStatusCode ? EdgeBoxStatusEnum.Active : EdgeBoxStatusEnum.Broken;
                    if (res.IsSuccessStatusCode)
                    {
                        if (edgeBoxInstall.EdgeBox.EdgeBoxStatusId != EdgeBoxStatusEnum.Active)
                        {
                            edgeBoxInstall.EdgeBoxInstallStatusId = EdgeBoxStatusEnum.Active;
                            await uow.CompleteAsync();
                        }
                    }
                    else
                        failedEdgeBoxInstall.Add(edgeBoxInstall);
                }
                catch (Exception ex) when (ex is TimeoutException || ex is HttpRequestException)
                {
                    logger.Error(ex.Message, ex);
                    failedEdgeBoxInstall.Add(edgeBoxInstall);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                    failedEdgeBoxInstall.Add(edgeBoxInstall);
                }
            }
            await HealthCheckAllFailedEdgeBox(failedEdgeBoxInstall, stoppingToken);
            try
            {
                await uow.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                await uow.RollBack();
            }
            await Task.Delay(
                TimeSpan.FromSeconds(
                    scope.ServiceProvider.GetRequiredService<HealthCheckConfiguration>().EdgeBoxHealthCheckDelay
                ),
                stoppingToken
            );
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
        var maxNumberOfTemptations = 5;
        while (failedEdgeBoxDic.Any() && !cancellation.IsCancellationRequested)
        {
            var startOperationTime = new TimeSpan(DateTime.Now.Ticks);
            foreach (var edgeBoxInstall in failedHealthCheckEdgeBoxes)
            {
                var currentNumberOfTemptations = failedEdgeBoxDic[edgeBoxInstall.Id].Item2;
                using var scope = provider.CreateScope();
                var edgeBoxService = scope.ServiceProvider.GetRequiredService<IEdgeBoxService>();
                if (currentNumberOfTemptations > maxNumberOfTemptations)
                {
                    failedEdgeBoxDic.Remove(edgeBoxInstall.Id);
                    await edgeBoxService.UpdateStatus(edgeBoxInstall.EdgeBoxId, EdgeBoxStatusEnum.Broken);
                    continue;
                }
                try
                {
                    var res = await SendRequest($"{edgeBoxInstall.IpAddress}/api/{edgeBoxInstall.Id}");
                    if (res.IsSuccessStatusCode)
                    {
                        failedEdgeBoxDic.Remove(edgeBoxInstall.Id);
                        await edgeBoxService.UpdateStatus(edgeBoxInstall.EdgeBoxId, EdgeBoxStatusEnum.Active);
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
