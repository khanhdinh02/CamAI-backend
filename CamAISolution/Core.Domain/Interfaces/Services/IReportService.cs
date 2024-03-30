using Core.Domain.DTO;
using Core.Domain.Enums;
using Core.Domain.Models;
using Core.Domain.Models.Consumers;

namespace Core.Domain.Interfaces.Services;

public interface IReportService
{
    public Task<ICircularBuffer<HumanCountModel>> GetHumanCountStream();
    public Task<ICircularBuffer<HumanCountModel>> GetHumanCountStream(Guid shopId);
    public Task<IEnumerable<HumanCountDto>> GetHumanCountData(DateOnly date, ReportTimeRange timeRange);
    public Task<IEnumerable<HumanCountDto>> GetHumanCountData(Guid shopId, DateOnly date, ReportTimeRange timeRange);
}
