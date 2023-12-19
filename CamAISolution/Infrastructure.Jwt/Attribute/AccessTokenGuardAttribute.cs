using Infrastructure.Jwt.Guard;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Jwt.Attribute;

public sealed class AccessTokenGuardAttribute : TypeFilterAttribute
{
    public AccessTokenGuardAttribute(string[] roles)
        : base(typeof(AccessTokenGuardFilter))
    {
        Arguments = new[] { roles };
    }
}
