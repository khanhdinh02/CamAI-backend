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
    /// If <c>edgeBoxInstall</c> has been fetched, use <see cref="UpdateStatus(Core.Domain.Entities.EdgeBoxInstall,Core.Domain.Enums.EdgeBoxInstallStatus,string?)"/> to avoid fetching again.
    /// </summary>
    Task<EdgeBoxInstall> UpdateStatus(Guid edgeBoxInstallId, EdgeBoxInstallStatus status, string? description = null);

    /// <summary>
    /// Use this method if <c>edgeBoxInstall</c> have been fetched to avoid fetching again, otherwise use <see cref="UpdateStatus(System.Guid,Core.Domain.Enums.EdgeBoxInstallStatus,string?)"/>.
    /// </summary>
    Task<EdgeBoxInstall> UpdateStatus(
        EdgeBoxInstall edgeBoxInstall,
        EdgeBoxInstallStatus status,
        string? description = null
    );

    Task<EdgeBoxInstall?> GetLatestInstallingByEdgeBox(Guid edgeBoxId);
    Task<PaginationResult<EdgeBoxInstall>> GetEdgeBoxInstall(SearchEdgeBoxInstallRequest searchRequest);

    /// <summary>
    /// Get all installs, that has been activated and not disabled by admin, of a shop.
    /// </summary>
    /// <param name="shopId"></param>
    /// <returns></returns>
    Task<PaginationResult<EdgeBoxInstall>> GetInstallingByShop(Guid shopId);

    Task<EdgeBoxInstall?> GetCurrentInstallationByShop(Guid shopId);

    /// <summary>
    /// Get all installs, that has been activated and not disabled by admin, of a brand.
    /// </summary>
    /// <param name="brandId"></param>
    /// <returns></returns>
    Task<PaginationResult<EdgeBoxInstall>> GetInstallingByBrand(Guid brandId);

    Task<EdgeBoxInstall> UpdateActivationStatus(
        Guid edgeBoxInstallId,
        EdgeBoxActivationStatus status,
        string? description = null
    );

    Task<EdgeBoxInstall> UpdateIpAddress(EdgeBoxInstall edgeBoxInstall, string ipAddress);
}
