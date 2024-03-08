using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Interfaces.Services;

public interface IEdgeBoxInstallService
{
    Task<EdgeBoxInstall> LeaseEdgeBox(CreateEdgeBoxInstallDto dto);
    Task<EdgeBoxInstall> ActivateEdgeBox(ActivateEdgeBoxDto dto);

    /// <summary>
    ///
    /// </summary>
    /// <param name="edgeBoxInstallId"></param>
    /// <param name="status"></param>
    /// <param name="edgeBoxInstall">If edgeBoxInstall object have been fetched, pass object instead to advoid querying again</param>
    /// <returns></returns>
    Task<EdgeBoxInstall> UpdateStatus(
        Guid edgeBoxInstallId,
        EdgeBoxInstallStatus status,
        EdgeBoxInstall? edgeBoxInstall = null
    );
}
