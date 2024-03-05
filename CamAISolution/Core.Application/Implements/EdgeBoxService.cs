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
            throw new NotFoundException(typeof(EdgeBox), id);
        return foundEdgeBox.Values[0];
    }

    public async Task<PaginationResult<EdgeBox>> GetEdgeBoxes(SearchEdgeBoxRequest searchRequest)
    {
        var edgeBoxes = await unitOfWork.EdgeBoxes.GetAsync(new EdgeBoxSearchSpec(searchRequest));
        return edgeBoxes;
    }

    public async Task<EdgeBox> CreateEdgeBox(CreateEdgeBoxDto edgeBoxDto)
    {
        // TODO: Check if the edge box model exists
        var edgeBox = mapping.Map<CreateEdgeBoxDto, EdgeBox>(edgeBoxDto);
        edgeBox.EdgeBoxStatus = EdgeBoxStatus.Active;
        edgeBox.EdgeBoxLocation = Domain.Enums.EdgeBoxLocation.Idle;
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

        mapping.Map(edgeBoxDto, foundEdgeBox);
        await unitOfWork.CompleteAsync();
        return await GetEdgeBoxById(id);
    }

    public async Task DeleteEdgeBox(Guid id)
    {
        var edgeBox = await unitOfWork.EdgeBoxes.GetByIdAsync(id);
        if (edgeBox == null)
            return;

        if (edgeBox.EdgeBoxLocation != EdgeBoxLocation.Idle)
            throw new ConflictException($"Cannot delete edge box that is not idle, id {id}");

        unitOfWork.EdgeBoxes.Delete(edgeBox);
        await unitOfWork.CompleteAsync();
    }

    public async Task UpdateStatus(Guid id, EdgeBoxStatus status)
    {
        var edgeBox = await unitOfWork.EdgeBoxes.GetByIdAsync(id);
        if (edgeBox != null && edgeBox.EdgeBoxStatus != status)
        {
            await unitOfWork.BeginTransaction();
            await unitOfWork.GetRepository<EdgeBoxActivity>().AddAsync(new EdgeBoxActivity
            {
                Description = $"Update status from {edgeBox.EdgeBoxStatus} to {status}",
                OldStatus = edgeBox.EdgeBoxStatus,
                EdgeBoxId = edgeBox.Id,
                NewStatus = status,
            });
            edgeBox.EdgeBoxStatus = status;

            unitOfWork.EdgeBoxes.Update(edgeBox);
            if (status == EdgeBoxStatus.Inactive)
            {
                var edgeboxInstalls = (await unitOfWork.GetRepository<EdgeBoxInstall>().GetAsync(expression: ei => ei.EdgeBoxId == id, takeAll: true)).Values;
                foreach (var ei in edgeboxInstalls)
                {
                    ei.EdgeBoxInstallStatus = EdgeBoxInstallStatus.Unhealthy;
                }
            }
            await unitOfWork.CommitTransaction();
        }
    }
}
