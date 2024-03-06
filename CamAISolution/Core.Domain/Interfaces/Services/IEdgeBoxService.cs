using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Services;

public interface IEdgeBoxService
{
    public Task<EdgeBox> GetEdgeBoxById(Guid id);
    public Task<PaginationResult<EdgeBox>> GetEdgeBoxes(SearchEdgeBoxRequest searchRequest);
    public Task<EdgeBox> CreateEdgeBox(CreateEdgeBoxDto edgeBoxDto);
    Task<EdgeBox> UpdateEdgeBox(Guid id, UpdateEdgeBoxDto edgeBoxDto);
    Task DeleteEdgeBox(Guid id);
    Task<IEnumerable<EdgeBox>> GetEdgeBoxesByShop(Guid shopId);
    Task<IEnumerable<EdgeBox>> GetEdgeBoxesByBrand(Guid brandId);
}
