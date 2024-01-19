using Core.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> TestEndpoint()
    {
        return Ok(new { data = $"Hello From {nameof(TestsController)}" });
    }

    [HttpGet("mq")]
    public async Task<IActionResult> SayHyToMQ()
    {
        var mqService = HttpContext.RequestServices.GetRequiredService<IMessageQueueService>();
        await mqService.SendMessage("This message must consume some where", "demo", "camai-exchange");
        return Ok();
    }
}
