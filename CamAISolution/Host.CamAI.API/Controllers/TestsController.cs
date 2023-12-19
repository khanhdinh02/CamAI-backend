using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API;

[Route("api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    [HttpGet]
    public IActionResult TestEndpoint()
    {
        return Ok(new { data = $"Hello From {nameof(TestsController)}" });
    }
}
