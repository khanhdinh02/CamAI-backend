using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface ICustomIncidentRepository : IRepository<Incident>
{
    void UpdateIncidentAssignment(Guid oldAssignmentId, Guid newAssignmentId);
}
