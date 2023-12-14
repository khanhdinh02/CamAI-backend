using System.Linq.Expressions;
using Core.Domain.Interfaces.Specifications;

namespace Core.Application.Specifications;
public abstract class Specification<T> : ISpecification<T>
{

    public bool IsSatisfied(T entity)
    {
        return ToExpression().Compile().Invoke(entity);
    }

    public abstract Expression<Func<T, bool>> ToExpression();

    public ISpecification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    public ISpecification<T> Not(ISpecification<T> specification)
    {
        return new NotSpecification<T>(specification);
    }

    public ISpecification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }
}

public class BaseSepcification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return _ => true;
    }
}
public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();

        var paramExpr = Expression.Parameter(typeof(T));
        var bodyExpr = Expression.AndAlso(leftExpr.Body, rightExpr.Body);
        bodyExpr = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(bodyExpr);
        return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
    }
}

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = left.ToExpression();
        var rightExpr = right.ToExpression();

        var paramExpr = Expression.Parameter(typeof(T));
        var bodyExpr = Expression.OrElse(leftExpr.Body, rightExpr.Body);
        bodyExpr = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(bodyExpr);
        return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
    }
}

public class NotSpecification<T>(ISpecification<T> specification) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expr = specification.ToExpression();
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
