using Core.Domain.DTO;
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
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task ShopCustomerAreaChart()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var classifierWs = new ClassifierWebSocket(webSocket, reportService);
            classifierWs.Start().Wait();
        }
        else
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }

    /// <summary>
    /// Only for brand manager
    /// This will get the real-time chart data for one of their shop
    /// </summary>
    [HttpGet("{shopId}/chart/customer")]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.BrandManager)]
    public async Task BrandCustomerAreaChart(Guid shopId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var classifierWs = new ClassifierWebSocket(webSocket, reportService, shopId);
            classifierWs.Start().Wait();
        }
        else
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    }

    /// <summary>
    /// Get past data of classifier for shop manager
    /// </summary>
    [HttpGet("customer")]
    // [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task<List<ClassifierModel>> GetClassifierData([FromQuery] DateOnly date)
    {
        return await reportService.GetClassifierDataForDate(date);
    }

    [HttpGet("{shopId}/customer")]
    // [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.BrandManager)]
    public async Task<List<ClassifierModel>> GetClassifierData([FromRoute] Guid shopId, [FromQuery] DateOnly date)
    {
        return await reportService.GetClassifierDataForDate(shopId, date);
    }
}
