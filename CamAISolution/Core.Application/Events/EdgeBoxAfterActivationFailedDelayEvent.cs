using Core.Application.Exceptions;
using Core.Application.Specifications;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Events;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;

namespace Core.Application.Events;

public class EdgeBoxAfterActivationFailedDelayEvent(TimeSpan delay, Guid edgeBoxId, Guid edgeBoxInstallId)
    : IApplicationDelayEvent
{
    public IUnitOfWork UnitOfWork { get; set; } = null!;
    public IEdgeBoxInstallService EdgeBoxInstallService { get; set; } = null!;
    public INotificationService NotificationService { get; set; } = null!;
    public IAppLogging<EdgeBoxAfterActivationFailedDelayEvent> Logger { get; set; } = null!;

    public Task UseDelay()
    {
        return Task.Delay(delay);
    }

    /// <summary>
    /// This method will check edge box install status and only send notification to Admin,
    /// because if activation is successful, the corresponding consumer will send notification brand manager.
    /// Thus, don't need to send notification to brand manager here.
    /// </summary>
    public async Task InvokeAsync()
    {
        try
        {
            var edgeBoxInstall =
                await UnitOfWork.EdgeBoxInstalls.GetByIdAsync(edgeBoxInstallId)
                ?? throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);
            var sentToAdmin = (
                await UnitOfWork.Accounts.GetAsync(new AccountByRoleSpec(Role.Admin).GetExpression(), takeAll: true)
            ).Values.Select(a => a.Id);

            if (edgeBoxInstall.ActivationStatus is not EdgeBoxActivationStatus.Activated)
            {
                await EdgeBoxInstallService.UpdateActivationStatus(
                    edgeBoxInstall.Id,
                    EdgeBoxActivationStatus.Failed,
                    "Edge box activation failed due to not receiving any response from edge box."
                );

                await NotificationService.CreateNotification(
                    new CreateNotificationDto
                    {
                        Content =
                            $"Edge box install #{edgeBoxInstallId} activation was failed due to not receiving any response from edge box.",
                        Title = "Edge box install activation was failed",
                        SentToId = sentToAdmin,
                        RelatedEntityId = edgeBoxInstallId,
                        Priority = NotificationPriority.Urgent,
                        Type = NotificationType.EdgeBoxInstallActivation
                    }
                );
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message, ex);
        }
    }
}
