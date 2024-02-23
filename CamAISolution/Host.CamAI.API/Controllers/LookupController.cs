using System.Reflection;
using Core.Application.Implements;
using Core.Domain.Enums;
using Host.CamAI.API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LookupController : ControllerBase
{
    [LookupHttpGet("account-statuses", typeof(AccountStatus))]
    [LookupHttpGet("brand-statuses", typeof(BrandStatus))]
    [LookupHttpGet("shop-statuses", typeof(ShopStatus))]
    [LookupHttpGet("edgeBox-locations", typeof(EdgeBoxLocation))]
    [LookupHttpGet("edgeBox-statuses", typeof(EdgeBoxStatus))]
    [LookupHttpGet("edgeBox-install-statuses", typeof(EdgeBoxInstallStatus))]
    [LookupHttpGet("employee-statuses", typeof(EmployeeStatus))]
    [LookupHttpGet("roles", typeof(Role))]
    [LookupHttpGet("short-days-of-week", typeof(ShortDayOfWeek))]
    [LookupHttpGet("genders", typeof(Gender))]
    [LookupHttpGet("notification-statuses", typeof(NotificationStatus))]
    [LookupHttpGet("notification-types", typeof(NotificationType))]
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
