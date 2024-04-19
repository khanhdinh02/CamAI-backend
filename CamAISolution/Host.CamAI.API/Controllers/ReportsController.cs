using Core.Domain.DTO;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api")]
[ApiController]
public class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>
    /// Only for shop manager
    /// This will get the real-time chart data for their shop
    /// </summary>
    [HttpGet("shops/chart/customer")]
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
    [HttpGet("shops/{shopId}/chart/customer")]
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
    [HttpGet("shops/customer")]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task<HumanCountDto> GetHumanCountData(DateOnly startDate, DateOnly endDate, ReportInterval interval)
    {
        return await reportService.GetHumanCountData(startDate, endDate, interval);
    }

    /// <summary>
    /// Get past data of human count for admin and brand manager
    /// </summary>
    [HttpGet("shops/{shopId}/customer")]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager)]
    public async Task<HumanCountDto> GetHumanCountData(
        [FromRoute] Guid shopId,
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] ReportInterval interval
    )
    {
        return await reportService.GetHumanCountData(shopId, startDate, endDate, interval);
    }

    [HttpGet("edgeboxes/report")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<EdgeBoxReportDto> EdgeBoxReport()
    {
        return await reportService.GetEdgeBoxReport();
    }

    [HttpGet("edgeBoxInstalls/report")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<EdgeBoxInstallReportDto> EdgeBoxInstallReport()
    {
        return await reportService.GetInstallEdgeBoxReport();
    }
}
