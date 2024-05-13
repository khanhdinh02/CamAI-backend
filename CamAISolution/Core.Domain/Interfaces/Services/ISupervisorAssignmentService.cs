using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface ISupervisorAssignmentService
{
    Task<SupervisorAssignment?> GetLatestAssignmentByDate(Guid shopId, DateTime date);
    Task<SupervisorAssignment?> GetLatestHeadSupervisorAssignmentByDate(Guid shopId, DateTime date);
    Task<IList<SupervisorAssignment>> GetSupervisorAssignmentByDate(DateTime date);
    Task<Account?> GetCurrentInChangeAccount(Guid shopId);
    Task<Account?> GetCurrentInChangeHeadSupervisorAccount(Guid shopId);
}
