using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface ISupervisorAssignmentService
{
    Task<SupervisorAssignment?> GetLatestSupervisorAssignment(Guid shopId);
    Task<SupervisorAssignment?> GetLatestHeadSupervisorAssignment(Guid shopId);
}
