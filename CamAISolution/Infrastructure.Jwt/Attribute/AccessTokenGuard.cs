using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Core.Domain.Models.enums;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Jwt.Guard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Jwt.Attribute;
public sealed class AccessTokenGuard : TypeFilterAttribute
{
    public AccessTokenGuard(string[] roles) : base(typeof(AccessTokenGuardFilter))
    {
        Arguments = new[] { roles };
    }

  
}
