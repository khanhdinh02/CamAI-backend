using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Consumers;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/shops")]
[ApiController]
public class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>
    /// Only for shop manager
    /// This will get the real-time chart data for their shop
    /// </summary>
    [HttpGet("chart/customer")]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task ShopCustomerAreaChart()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var humanCountWs = new HumanCountWebSocket(webSocket, reportService);
            humanCountWs.Start().Wait();
        }
        else
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }

    /// <summary>
    /// Only for brand manager
    /// This will get the real-time chart data for one of their shop
    /// </summary>
    [HttpGet("{shopId}/chart/customer")]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager)]
    public async Task BrandCustomerAreaChart(Guid shopId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var humanCountWs = new HumanCountWebSocket(webSocket, reportService, shopId);
            humanCountWs.Start().Wait();
        }
        else
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }

    /// <summary>
    /// Get past data of human count for shop manager
    /// </summary>
    [HttpGet("customer")]
    // [AccessTokenGuard(Role.ShopManager)]
    public async Task<List<HumanCountModel>> GetHumanCountData([FromQuery] DateOnly date)
    {
        return await reportService.GetHumanCountDataForDate(date);
    }

    [HttpGet("{shopId}/customer")]
    // [AccessTokenGuard(Role.ShopManager, Role.BrandManager)]
    public async Task<List<HumanCountModel>> GetHumanCountData([FromRoute] Guid shopId, [FromQuery] DateOnly date)
    {
        return await reportService.GetHumanCountDataForDate(shopId, date);
    }
}
