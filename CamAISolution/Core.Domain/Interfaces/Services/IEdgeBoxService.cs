using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IEdgeBoxService
{
    Task<EdgeBox> GetEdgeBoxById(Guid id);
    Task<IEnumerable<EdgeBox>> GetEdgeBoxes();
    Task<PaginationResult<EdgeBox>> GetEdgeBoxes(SearchEdgeBoxRequest searchRequest);
    Task<EdgeBox> CreateEdgeBox(CreateEdgeBoxDto edgeBoxDto);
    Task<EdgeBox> UpdateEdgeBox(Guid id, UpdateEdgeBoxDto edgeBoxDto);
    Task DeleteEdgeBox(Guid id);
    Task UpdateStatus(Guid id, int statusId);
}
