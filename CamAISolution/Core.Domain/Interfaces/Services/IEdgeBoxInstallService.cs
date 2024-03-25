using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models;

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

    Task<EdgeBoxInstall?> GetLatestInstallingByEdgeBox(Guid edgeBoxId);
    Task<PaginationResult<EdgeBoxInstall>> GetEdgeBoxInstall(SearchEdgeBoxInstallRequest searchRequest);

    /// <summary>
    /// Get all installs, that has been activated and not disabled by admin, of a shop.
    /// </summary>
    /// <param name="shopId"></param>
    /// <returns></returns>
    Task<IEnumerable<EdgeBoxInstall>> GetInstallingByShop(Guid shopId);

    /// <summary>
    /// Get all installs, that has been activated and not disabled by admin, of a brand.
    /// </summary>
    /// <param name="brandId"></param>
    /// <returns></returns>
    Task<IEnumerable<EdgeBoxInstall>> GetInstallingByBrand(Guid brandId);
}
