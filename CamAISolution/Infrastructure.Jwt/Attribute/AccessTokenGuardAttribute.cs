using Core.Domain.Enums;
using Infrastructure.Jwt.Guard;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Jwt.Attribute;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class AccessTokenGuardAttribute : TypeFilterAttribute
{
    public AccessTokenGuardAttribute(params Role[] roles)
        : base(typeof(AccessTokenGuardFilter))
    {
        Arguments = [roles, false];
    }

    public AccessTokenGuardAttribute(bool allowNew, Role[]? roles)
        : base(typeof(AccessTokenGuardFilter))
    {
        Arguments = [roles, allowNew];
    }
}
