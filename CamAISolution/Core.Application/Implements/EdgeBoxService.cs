using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class EdgeBoxService(IUnitOfWork unitOfWork, IAccountService accountService, IBaseMapping mapping)
    : IEdgeBoxService
{
    public async Task<EdgeBox> GetEdgeBoxById(Guid id)
    {
        var foundEdgeBox = await unitOfWork.EdgeBoxes.GetAsync(new EdgeBoxByIdRepoSpec(id));
        if (foundEdgeBox.Values.Count == 0)
        {
            throw new NotFoundException(typeof(EdgeBox), id);
        }

        return foundEdgeBox.Values[0];
    }

    public async Task<PaginationResult<EdgeBox>> GetEdgeBoxes(SearchEdgeBoxRequest searchRequest)
    {
        var crit = new EdgeBoxSearchSpec(searchRequest).Criteria;
        IEnumerable<EdgeBox> edgeBoxes = (
            await unitOfWork.EdgeBoxes.GetAsync(crit, takeAll: true, includeProperties: [nameof(EdgeBox.EdgeBoxModel)])
        ).Values;
        if (searchRequest.BrandId.HasValue)
        {
            edgeBoxes = edgeBoxes.IntersectBy(
                (await GetEdgeBoxesByBrand(searchRequest.BrandId.Value)).Select(eb => eb.Id),
                eb => eb.Id
            );
        }

        if (searchRequest.ShopId.HasValue)
        {
            edgeBoxes = edgeBoxes.IntersectBy(
                (await GetEdgeBoxesByShop(searchRequest.ShopId.Value)).Select(eb => eb.Id),
                eb => eb.Id
            );
        }

        return new PaginationResult<EdgeBox>(edgeBoxes, searchRequest.PageIndex, searchRequest.Size);
    }

    public async Task<IEnumerable<EdgeBox>> GetEdgeBoxesByShop(Guid shopId)
    {
        // The edge box is currently installed in a shop if EdgeBoxLocation is neither Idle nor Disposed
        // The current shop that the edge box is installed in is the shop that has the latest installation record
        return (
            await unitOfWork.EdgeBoxes.GetAsync(
                eb => eb.EdgeBoxLocation != EdgeBoxLocation.Idle,
                null,
                [nameof(EdgeBox.Installs), nameof(EdgeBox.EdgeBoxModel)],
                true,
                true
            )
        )
            .Values.Where(eb => eb.Installs.MaxBy(i => i.CreatedDate)?.ShopId == shopId)
            .ToList();
    }

    public async Task<IEnumerable<EdgeBox>> GetEdgeBoxesByBrand(Guid brandId)
    {
        // The edge box is currently installed in a Brand if EdgeBoxLocation is neither Idle nor Disposed
        // The current Brand that the edge box is installed in is the Brand that has the latest installation record
        return (
            await unitOfWork.EdgeBoxes.GetAsync(
                eb => eb.EdgeBoxLocation != EdgeBoxLocation.Idle,
                null,
                [$"{nameof(EdgeBox.Installs)}.{nameof(EdgeBoxInstall.Shop)}", nameof(EdgeBox.EdgeBoxModel)],
                true,
                true
            )
        )
            .Values.Where(eb => eb.Installs.MaxBy(i => i.CreatedDate)?.Shop?.BrandId == brandId)
            .ToList();
    }

    public async Task<EdgeBox> CreateEdgeBox(CreateEdgeBoxDto edgeBoxDto)
    {
        if (!(await unitOfWork.EdgeBoxes.GetAsync(x => x.SerialNumber == edgeBoxDto.SerialNumber)).IsValuesEmpty)
            throw new BadRequestException("Serial number already exist in the system");

        _ =
            await unitOfWork.EdgeBoxModels.GetByIdAsync(edgeBoxDto.EdgeBoxModelId)
            ?? throw new NotFoundException(typeof(EdgeBoxModel), edgeBoxDto.EdgeBoxModelId);

        var edgeBox = mapping.Map<CreateEdgeBoxDto, EdgeBox>(edgeBoxDto);
        edgeBox.EdgeBoxStatus = EdgeBoxStatus.Active;
        edgeBox.EdgeBoxLocation = EdgeBoxLocation.Idle;
        edgeBox = await unitOfWork.EdgeBoxes.AddAsync(edgeBox);
        await unitOfWork.CompleteAsync();
        return edgeBox;
    }

    public async Task<EdgeBox> UpdateEdgeBox(Guid id, UpdateEdgeBoxDto edgeBoxDto)
    {
        var foundEdgeBoxes = await unitOfWork.EdgeBoxes.GetAsync(new EdgeBoxByIdRepoSpec(id));
        if (foundEdgeBoxes.Values.Count == 0)
            throw new NotFoundException(typeof(EdgeBox), id);

        var foundEdgeBox = foundEdgeBoxes.Values[0];
        var currentAccount = accountService.GetCurrentAccount();
        if (foundEdgeBox.EdgeBoxStatus == EdgeBoxStatus.Inactive && currentAccount.Role != Role.Admin)
            throw new BadRequestException("Cannot modified inactive edgeBox");

        if (
            foundEdgeBox.SerialNumber != edgeBoxDto.SerialNumber
            && !(await unitOfWork.EdgeBoxes.GetAsync(x => x.SerialNumber == edgeBoxDto.SerialNumber)).IsValuesEmpty
        )
            throw new BadRequestException("Serial number already exist in the system");

        _ =
            await unitOfWork.EdgeBoxModels.GetByIdAsync(edgeBoxDto.EdgeBoxModelId)
            ?? throw new NotFoundException(typeof(EdgeBoxModel), edgeBoxDto.EdgeBoxModelId);

        mapping.Map(edgeBoxDto, foundEdgeBox);
        await unitOfWork.CompleteAsync();
        return await GetEdgeBoxById(id);
    }

    public async Task DeleteEdgeBox(Guid id)
    {
        var edgeBox = (
            await unitOfWork.EdgeBoxes.GetAsync(eb => eb.Id == id, includeProperties: [nameof(EdgeBox.Installs)])
        ).Values.FirstOrDefault();

        if (edgeBox == null)
            return;

        if (edgeBox.Installs.Count == 0)
        {
            unitOfWork.EdgeBoxes.Delete(edgeBox);
            await unitOfWork.CompleteAsync();
        }
        else
        {
            if (edgeBox.EdgeBoxLocation != EdgeBoxLocation.Idle)
                throw new ConflictException($"Cannot delete edge box that is not idle, id {id}");
            await UpdateStatus(edgeBox.Id, EdgeBoxStatus.Disposed, "Dispose edge box");
        }
    }

    public async Task UpdateStatus(Guid id, EdgeBoxStatus status, string? description = null)
    {
        var edgeBox = await unitOfWork.EdgeBoxes.GetByIdAsync(id) ?? throw new NotFoundException(typeof(EdgeBox), id);
        if (edgeBox.EdgeBoxStatus == status)
            return;

        // Active -> *, broken -> disposed: check no edge box install valid
        // Active -> broken: allow
        if (
            (edgeBox.EdgeBoxStatus == EdgeBoxStatus.Active && status != EdgeBoxStatus.Broken)
            || (edgeBox.EdgeBoxStatus == EdgeBoxStatus.Broken && status == EdgeBoxStatus.Disposed)
        )
        {
            // Temporary fix for circular dependency
            var latestInstallingByEdgeBox = (
                await unitOfWork.EdgeBoxInstalls.GetAsync(
                    i => i.EdgeBoxId == edgeBox.Id && i.EdgeBox.EdgeBoxLocation != EdgeBoxLocation.Idle,
                    o => o.OrderByDescending(i => i.CreatedDate),
                    pageSize: 1
                )
            ).Values.FirstOrDefault();
            if (
                latestInstallingByEdgeBox != null
                && latestInstallingByEdgeBox.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled
            )
                throw new BadRequestException("Cannot change status of edge box that is installing");
        }

        await unitOfWork.EdgeBoxActivities.AddAsync(
            new EdgeBoxActivity
            {
                Type = EdgeBoxActivityType.EdgeBoxStatus,
                Description = description ?? $"Update status from {edgeBox.EdgeBoxStatus} to {status}",
                EdgeBoxId = edgeBox.Id
            }
        );

        edgeBox.EdgeBoxStatus = status;
        unitOfWork.EdgeBoxes.Update(edgeBox);

        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateLocation(Guid id, EdgeBoxLocation location, string? description = null)
    {
        var edgeBox = await unitOfWork.EdgeBoxes.GetByIdAsync(id) ?? throw new NotFoundException(typeof(EdgeBox), id);
        if (edgeBox.EdgeBoxLocation == location)
            return;

        switch (edgeBox.EdgeBoxLocation)
        {
            // installing -> occupied and installing --> idle
            case EdgeBoxLocation.Installing when location is EdgeBoxLocation.Occupied or EdgeBoxLocation.Idle:
            // occupied --> uninstalling
            case EdgeBoxLocation.Occupied when location == EdgeBoxLocation.Uninstalling:
            // uninstalling -> idle
            case EdgeBoxLocation.Uninstalling when location == EdgeBoxLocation.Idle:
            // idle --> installing
            case EdgeBoxLocation.Idle when location == EdgeBoxLocation.Installing:
                await unitOfWork.EdgeBoxActivities.AddAsync(
                    new EdgeBoxActivity
                    {
                        Type = EdgeBoxActivityType.EdgeBoxLocation,
                        EdgeBoxId = edgeBox.Id,
                        Description = description ?? $"Update location from {edgeBox.EdgeBoxLocation} to {location}"
                    }
                );

                edgeBox.EdgeBoxLocation = location;
                unitOfWork.EdgeBoxes.Update(edgeBox);

                await unitOfWork.CompleteAsync();
                break;
            default:
                throw new ForbiddenException(
                    $"Cannot update current location {edgeBox.EdgeBoxLocation} to location {location}"
                );
        }
    }

    public async Task<PaginationResult<EdgeBoxActivity>> GetActivitiesByEdgeBoxId(
        Guid edgeBoxId,
        SearchEdgeBoxActivityRequest req
    )
    {
        return await unitOfWork.EdgeBoxActivities.GetAsync(
            a => a.EdgeBoxId == edgeBoxId || a.EdgeBoxInstall!.EdgeBoxId == edgeBoxId,
            orderBy: q => q.OrderByDescending(a => a.ModifiedTime),
            includeProperties: [nameof(EdgeBoxActivity.ModifiedBy)],
            pageSize: req.Size,
            pageIndex: req.PageIndex
        );
    }
}
