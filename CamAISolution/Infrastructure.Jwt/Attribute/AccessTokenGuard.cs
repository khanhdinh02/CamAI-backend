using Microsoft.AspNetCore.Mvc;
using Infrastructure.Jwt.Guard;

namespace Infrastructure.Jwt.Attribute;
public sealed class AccessTokenGuard : TypeFilterAttribute
{
    public AccessTokenGuard(string[] roles) : base(typeof(AccessTokenGuardFilter))
    {
        Arguments = new[] { roles };
    }


}
