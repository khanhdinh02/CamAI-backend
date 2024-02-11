using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.Configurations;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService(IAppLogging<EdgeBoxHealthCheckService> logger, IServiceProvider provider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = provider.CreateScope();
            var edgeBoxService = scope.ServiceProvider.GetRequiredService<IEdgeBoxService>();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var edgeBoxeInstallRepo = uow.GetRepository<EdgeBoxInstall>();
            var edgeBoxInstalls = (
                await edgeBoxeInstallRepo.GetAsync(
                    expression: e =>
                        e.EdgeBoxInstallStatusId == EdgeBoxInstallStatusEnum.Valid
                        && e.EdgeBox.EdgeBoxStatusId == EdgeBoxStatusEnum.Active,
                    takeAll: true,
                    includeProperties: [nameof(EdgeBoxActivity.EdgeBox)]
                )
            ).Values;
            foreach (var edgeBoxInstall in edgeBoxInstalls)
            {
                int? newStatusId = null;
                try
                {
                    var res = await SendRequest($"{edgeBoxInstall.IpAddress}/api/{edgeBoxInstall.Id}");
                    if (res.IsSuccessStatusCode)
                        newStatusId = EdgeBoxStatusEnum.Active;
                    else
                        newStatusId = EdgeBoxStatusEnum.Broken;
                    if (newStatusId.Value != edgeBoxInstall.EdgeBox.EdgeBoxStatusId)
                    {
                        edgeBoxInstall.EdgeBox.EdgeBoxStatusId = newStatusId.Value;
                        await uow.CompleteAsync();
                    }
                }
                catch (Exception ex) when (ex is TimeoutException || ex is HttpRequestException)
                {
                    logger.Error(ex.Message, ex);
                    await edgeBoxService.UpdateStatus(edgeBoxInstall.EdgeBoxId, EdgeBoxStatusEnum.Broken);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }
            await Task.Delay(
                TimeSpan.FromSeconds(
                    scope.ServiceProvider.GetRequiredService<HealthCheckConfiguration>().EdgeBoxHealthCheckDelay
                ),
                stoppingToken
            );
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
