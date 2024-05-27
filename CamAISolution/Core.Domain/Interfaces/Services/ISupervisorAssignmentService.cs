using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface ISupervisorAssignmentService
{
    Task<SupervisorAssignment?> GetLatestAssignmentByDate(Guid shopId, DateTime date, bool includeAll = true);
    Task<IList<SupervisorAssignment>> GetSupervisorAssignmentByDate(DateTime date);
    Task<Account?> GetCurrentInChangeAccount(Guid shopId);
    Task RemoveSupervisor();
}
