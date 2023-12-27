using Infrastructure.Jwt.Guard;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Jwt.Attribute;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class AccessTokenGuardAttribute : TypeFilterAttribute
{
    public AccessTokenGuardAttribute(params int[] roleIds)
        : base(typeof(AccessTokenGuardFilter))
    {
        Arguments = new[] { roleIds };
    }
}
