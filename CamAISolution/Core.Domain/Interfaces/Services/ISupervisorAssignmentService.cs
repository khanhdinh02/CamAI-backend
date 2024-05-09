using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface ISupervisorAssignmentService
{
    Task<SupervisorAssignment?> GetLatestAssignmentByDate(Guid shopId, DateTime date);
    Task<SupervisorAssignment?> GetLatestHeadSupervisorAssignmentByDate(Guid shopId, DateTime date);
    Task<Account?> GetCurrentInChangeAccount(Guid shopId);
}
