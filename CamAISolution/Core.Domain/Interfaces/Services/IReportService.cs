using Core.Domain.Models;
using Core.Domain.Models.Consumers;

namespace Core.Domain.Interfaces.Services;

public interface IReportService
{
    public Task<ICircularBuffer<HumanCountModel>> GetHumanCountStream();
    public Task<ICircularBuffer<HumanCountModel>> GetHumanCountStream(Guid shopId);
    public Task<List<HumanCountModel>> GetHumanCountDataForDate(DateOnly date);
    public Task<List<HumanCountModel>> GetHumanCountDataForDate(Guid shopId, DateOnly date);
}
