using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using Host.CamAI.API.BackgroundServices;
using Host.CamAI.API.Consumers.Contracts;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API.Consumers;

[Consumer("{MachineName}_HealthCheckResponse", ConsumerConstant.HealthCheckResponse)]
public class HealthCheckResponseConsumer(
    IAppLogging<HealthCheckResponseConsumer> logger,
    IEdgeBoxInstallService edgeBoxInstallService,
    IJwtService jwtService
) : IConsumer<HealthCheckResponseMessage>
{
    public async Task Consume(ConsumeContext<HealthCheckResponseMessage> context)
    {
        var message = context.Message;
        logger.Info($"Receive health check response from edge box {message.EdgeBoxId}");
        EdgeBoxHealthCheckService.ReceivedEdgeBoxHealthResponse(message.EdgeBoxId);
        var ebInstall = await edgeBoxInstallService.GetLatestInstallingByEdgeBox(message.EdgeBoxId);
        if (ebInstall == null)
        {
            logger.Info($"Edge box install not found for {message.EdgeBoxId}");
            return;
        }
        if (ebInstall.EdgeBoxInstallStatus == message.Status)
        {
            logger.Info($"Edge box install status not change {message.Status}");
            return;
        }

        // TODO: remove jwt service after remove modified by in activity
        await jwtService.SetCurrentUserToSystemHandler();
        // TODO: add reason after refactor activity
        await edgeBoxInstallService.UpdateStatus(ebInstall, message.Status, message.Reason);

        if (message.Status == EdgeBoxInstallStatus.Unhealthy)
        {
            // TODO: notify admin and manager
        }
        else
        {
            // TODO: try to reactivate edge box
            // TODO: notify admin and manager as well
        }
    }
}
