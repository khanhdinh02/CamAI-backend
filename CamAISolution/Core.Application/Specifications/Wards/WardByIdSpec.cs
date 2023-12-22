using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class WardByIdSpec : Specification<Ward>
{
    private readonly Guid id;

    public WardByIdSpec(Guid id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Ward, bool>> GetExpression()
    {
        return w => w.Id == id;
    }
}
