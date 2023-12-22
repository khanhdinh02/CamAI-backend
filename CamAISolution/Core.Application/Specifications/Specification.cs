using System.Linq.Expressions;
using Core.Domain.Specifications;

namespace Core.Application.Specifications;
public class Specification<T> : ISpecification<T>
{
    protected Expression<Func<T, bool>> Expr = _ => true;

    public bool IsSatisfied(T entity)
    {
        return GetExpression().Compile().Invoke(entity);
    }

    public virtual Expression<Func<T, bool>> GetExpression()
    {
        return Expr;
    }

    public ISpecification<T> And(ISpecification<T> specification)
    {
        var spec = new AndSpecification<T>(this, specification);
        Expr = spec.GetExpression();
        return spec;
    }

    public ISpecification<T> Not(ISpecification<T> specification)
    {
        var spec = new NotSpecification<T>(specification);
        Expr = spec.GetExpression();
        return spec;
    }

    public ISpecification<T> Or(ISpecification<T> specification)
    {
        var spec = new OrSpecification<T>(this, specification);
        Expr = spec.GetExpression();
        return spec;
    }
}

//public class BaseSpecification<T> : Specification<T>, ISpecification<T>
//{
//    public override Expression<Func<T, bool>> GetExpression()
//    {
//        return expr;
//    }
//}
public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>()
{
    public override Expression<Func<T, bool>> GetExpression()
    {
        var leftExpr = left.GetExpression();
        var rightExpr = right.GetExpression();

        var paramExpr = Expression.Parameter(typeof(T));
        var bodyExpr = Expression.AndAlso(leftExpr.Body, rightExpr.Body);
        bodyExpr = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(bodyExpr);
        return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
    }
}

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>()
{
    public override Expression<Func<T, bool>> GetExpression()
    {
        var leftExpr = left.GetExpression();
        var rightExpr = right.GetExpression();

        var paramExpr = Expression.Parameter(typeof(T));
        var bodyExpr = Expression.OrElse(leftExpr.Body, rightExpr.Body);
        bodyExpr = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(bodyExpr);
        return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
    }
}

public class NotSpecification<T>(ISpecification<T> specification) : Specification<T>()
{
    public override Expression<Func<T, bool>> GetExpression()
    {
        var expr = specification.GetExpression();
        var paramExpr = Expression.Parameter(typeof(T));
        var bodyExpr = Expression.Not(expr.Body);
        return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
    }
}

internal class ParameterReplacer(ParameterExpression parameter) : ExpressionVisitor
{

    protected override Expression VisitParameter(ParameterExpression node)
        => base.VisitParameter(parameter);
}
