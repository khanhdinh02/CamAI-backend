using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class EdgeBoxModelService(IUnitOfWork unitOfWork) : IEdgeBoxModelService
{
    public async Task<IEnumerable<EdgeBoxModel>> GetAll()
    {
        return (await unitOfWork.EdgeBoxModels.GetAsync(takeAll: true)).Values;
    }
}
