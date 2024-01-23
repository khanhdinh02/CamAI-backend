using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Specifications;

public class WardByIdSpec : Specification<Ward>
{
    private readonly int id;

    public WardByIdSpec(int id)
    {
        this.id = id;
        Expr = GetExpression();
    }

    public override Expression<Func<Ward, bool>> GetExpression()
    {
        return w => w.Id == id;
    }
}
