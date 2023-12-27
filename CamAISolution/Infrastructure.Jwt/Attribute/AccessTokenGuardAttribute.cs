using Infrastructure.Jwt.Guard;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Jwt.Attribute;

public sealed class AccessTokenGuardAttribute : TypeFilterAttribute
{
    public AccessTokenGuardAttribute(Guid[] roleGuid)
        : base(typeof(AccessTokenGuardFilter))
    {
        Arguments = new[] { roleGuid };
    }
}
