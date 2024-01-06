using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class EdgeBoxService(
    IUnitOfWork unitOfWork,
    IAccountService accountService,
    IAppLogging<EdgeBoxService> logger,
    IBaseMapping mapping
) : IEdgeBoxService
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
        var edgeBox = mapping.Map<CreateEdgeBoxDto, EdgeBox>(edgeBoxDto);
        edgeBox.EdgeBoxStatusId = EdgeBoxStatusEnum.Active;
        edgeBox.EdgeBoxLocationId = EdgeBoxLocationEnum.Idle;
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
        var currentAccount = await accountService.GetCurrentAccount();
        if (foundEdgeBox.EdgeBoxStatusId == EdgeBoxStatusEnum.Inactive && !currentAccount.HasRole(RoleEnum.Admin))
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

        if (edgeBox.EdgeBoxLocationId != EdgeBoxLocationEnum.Idle)
            throw new ConflictException($"Cannot delete edge box that is not idle, id {id}");

        unitOfWork.EdgeBoxes.Delete(edgeBox);
        await unitOfWork.CompleteAsync();
    }
}
