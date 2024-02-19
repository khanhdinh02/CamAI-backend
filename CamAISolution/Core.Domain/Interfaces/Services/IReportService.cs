using Core.Domain.Models;
using Core.Domain.Models.Consumers;

namespace Core.Domain.Interfaces.Services;

public interface IReportService
{
    public Task<ICircularBuffer<ClassifierModel>> GetClassifierStream();
    public Task<ICircularBuffer<ClassifierModel>> GetClassifierStream(Guid shopId);
    public Task<List<ClassifierModel>> GetClassifierDataForDate(DateOnly date);
    public Task<List<ClassifierModel>> GetClassifierDataForDate(Guid shopId, DateOnly date);
}
