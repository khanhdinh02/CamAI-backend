using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;

public class CustomIncidentRepository(
    CamAIContext context,
    IRepositorySpecificationEvaluator<Incident> specificationEvaluator
) : Repository<Incident>(context, specificationEvaluator), ICustomIncidentRepository
{
    public void UpdateIncidentAssignment(Guid oldAssignmentId, Guid newAssignmentId)
    {
        Context
            .Set<Incident>()
            .Where(x => x.AssignmentId == oldAssignmentId)
            .ExecuteUpdate(x => x.SetProperty(i => i.AssignmentId, newAssignmentId));
    }
}
