using System.Reflection;
using Core.Application.Implements;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Host.CamAI.API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LookupController : ControllerBase
{
    [LookupHttpGet("account-status", typeof(AccountStatusEnum))]
    [LookupHttpGet("brand-status", typeof(BrandStatusEnum))]
    [LookupHttpGet("shop-status", typeof(ShopStatusEnum))]
    [LookupHttpGet("edgeBox-location", typeof(EdgeBoxLocationEnum))]
    [LookupHttpGet("edgeBox-status", typeof(EdgeBoxStatusEnum))]
    [LookupHttpGet("edgeBox-install-status", typeof(EdgeBoxInstallStatusEnum))]
    [LookupHttpGet("role", typeof(RoleEnum))]
    [LookupHttpGet("gender", typeof(Gender))]
    [LookupHttpGet("notification-status", typeof(NotificationStatusEnum))]
    [LookupHttpGet("notification-type", typeof(NotificationTypeEnum))]
    public ActionResult<Dictionary<int, string>> GetLookup()
    {
        var path = HttpContext.Request.Path;
        var lookupType = typeof(LookupController)
            .GetMethod(nameof(GetLookup))!
            .GetCustomAttributes<LookupHttpGetAttribute>(false)
            .FirstOrDefault(x => path.ToString().Contains(x.Template!))
            ?.Type;

        if (lookupType != null)
            return Ok(LookupService.GetLookupValues(lookupType));

        return Ok();
    }
}
