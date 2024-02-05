using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Services;

namespace Host.CamAI.API.BackgroundServices;

public class EdgeBoxHealthCheckService : BackgroundService
{
    private readonly IEdgeBoxService edgeBoxService;
    private readonly IAppLogging<EdgeBoxHealthCheckService> logger;

    public EdgeBoxHealthCheckService(IEdgeBoxService edgeBoxService, IAppLogging<EdgeBoxHealthCheckService> logger)
    {
        this.edgeBoxService = edgeBoxService;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Get All Edgebox
        while (!stoppingToken.IsCancellationRequested)
        {
            var edgeBoxes = await edgeBoxService.GetEdgeBoxes();
            foreach (var edgeBox in edgeBoxes)
            {
                try
                {
                    var http = new HttpClient();
                    http.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                    );
                    var res = await http.GetAsync(new Uri($"{edgeBox.HostingAddress}/api/{edgeBox.Id}"));
                    if (res.IsSuccessStatusCode)
                        await edgeBoxService.UpdateStatus(edgeBox.Id, EdgeBoxStatusEnum.Active);
                    else
                        await edgeBoxService.UpdateStatus(edgeBox.Id, EdgeBoxStatusEnum.Inactive);
                }
                catch (Exception ex) when (ex is TimeoutException || ex is HttpRequestException)
                {
                    await edgeBoxService.UpdateStatus(edgeBox.Id, EdgeBoxStatusEnum.Broken);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
