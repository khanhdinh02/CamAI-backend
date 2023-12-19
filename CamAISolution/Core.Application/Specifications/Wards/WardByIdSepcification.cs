using System.Linq.Expressions;
using Core.Application.Specifications;
using Core.Domain.Entities;

namespace Core.Application;

public class WardByIdSepcification : Specification<Ward>
{
    private readonly Guid id;
    public WardByIdSepcification(Guid id)
    {
        this.id = id;
        expr = GetExpression();
    }

    public override Expression<Func<Ward, bool>> GetExpression()
    {
        return w => w.Id == id;
    }
}
