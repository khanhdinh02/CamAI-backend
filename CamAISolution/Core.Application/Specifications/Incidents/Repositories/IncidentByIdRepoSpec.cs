using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Incidents.Repositories;

public class IncidentByIdRepoSpec : EntityByIdSpec<Incident, Guid>
{
    public IncidentByIdRepoSpec(Guid id, bool includeAll = true)
        : base(x => x.Id == id)
    {
        AddIncludes(x => x.Shop);

        if (includeAll)
        {
            AddIncludes("Evidences.Image");
            AddIncludes(x => x.Employee, x => x.Assignment);
        }
    }
}
