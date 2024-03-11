using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Interfaces.Services;

public interface IEdgeBoxInstallService
{
    Task<EdgeBoxInstall> LeaseEdgeBox(CreateEdgeBoxInstallDto dto);
    Task<EdgeBoxInstall> ActivateEdgeBox(ActivateEdgeBoxDto dto);

    /// <summary>
    /// If <c>edgeBoxInstall</c> has been fetched, use <see cref="UpdateStatus(EdgeBoxInstall, EdgeBoxInstallStatus)"/> to avoid fetching again.
    /// </summary>
    Task<EdgeBoxInstall> UpdateStatus(Guid edgeBoxInstallId, EdgeBoxInstallStatus status);

    /// <summary>
    /// Use this method if <c>edgeBoxInstall</c> have been fetched to avoid fetching again, otherwise use <see cref="UpdateStatus(Guid, EdgeBoxInstallStatus)"/>.
    /// </summary>
    Task<EdgeBoxInstall> UpdateStatus(EdgeBoxInstall edgeBoxInstall, EdgeBoxInstallStatus status);

    Task<EdgeBoxInstall?> GetInstallingByEdgeBox(Guid edgeBoxId);
}
