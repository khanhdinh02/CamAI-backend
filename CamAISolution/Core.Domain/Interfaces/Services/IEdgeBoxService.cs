using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IEdgeBoxService
{
    Task<EdgeBox> GetEdgeBoxById(Guid id);
    Task<PaginationResult<EdgeBox>> GetEdgeBoxes(SearchEdgeBoxRequest searchRequest);
    Task<EdgeBox> CreateEdgeBox(CreateEdgeBoxDto edgeBoxDto);
    Task<EdgeBox> UpdateEdgeBox(Guid id, UpdateEdgeBoxDto edgeBoxDto);
    Task DeleteEdgeBox(Guid id);
    Task UpdateStatus(Guid id, EdgeBoxStatus status);
    Task<IEnumerable<EdgeBox>> GetEdgeBoxesByShop(Guid shopId);
    Task<IEnumerable<EdgeBox>> GetEdgeBoxesByBrand(Guid brandId);
}