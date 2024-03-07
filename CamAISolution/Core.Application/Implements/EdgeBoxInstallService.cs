using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;

namespace Core.Application.Implements;
public class EdgeBoxInstallService(IUnitOfWork uow) : IEdgeBoxInstallService
{
    public async Task<EdgeBoxInstall> UpdateStatus(Guid edgeBoxInstallId, EdgeBoxInstallStatus status, EdgeBoxInstall? edgeBoxInstall = null)
    {
        edgeBoxInstall = edgeBoxInstall ?? await uow.GetRepository<EdgeBoxInstall>().GetByIdAsync(edgeBoxInstallId) ?? throw new NotFoundException(typeof(EdgeBoxInstall), edgeBoxInstallId);
        if (edgeBoxInstall.EdgeBoxInstallStatus != status)
        {
            await uow.BeginTransaction();
            await uow.GetRepository<EdgeBoxInstallActivity>().AddAsync(new()
            {
                Description = $"Update status from {edgeBoxInstall.EdgeBoxInstallStatus} to {status}",
                NewStatus = status,
                OldStatus = edgeBoxInstall.EdgeBoxInstallStatus,
                EdgeBoxInstallId = edgeBoxInstallId,
            });
            edgeBoxInstall.EdgeBoxInstallStatus = status;
            uow.GetRepository<EdgeBoxInstall>().Update(edgeBoxInstall);

            await uow.CommitTransaction();
        }
        return edgeBoxInstall;
    }
}
