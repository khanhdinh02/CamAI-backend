using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestsController(IBaseMapping mapper, IRequestService requestService) : ControllerBase
{
    /// <summary>
    /// Search requests.<br/>
    /// If user is Brand Manager, only requests of the brand will be returned.<br/>
    /// If user is Shop Manager, only requests of the shop will be returned.
    /// </summary>
    /// <remarks>
    /// Allowed roles: Admin, BrandManager, ShopManager.
    /// </remarks>
    /// <seealso cref="GetPersonalRequests"/>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<PaginationResult<RequestDto>> GetRequests([FromQuery] SearchRequestRequest req)
    {
        var requests = await requestService.GetRequests(req);
        return mapper.Map<Request, RequestDto>(requests);
    }

    /// <summary>
    /// Search current user's requests that are not related to any shop or edge box.
    /// </summary>
    /// <seealso cref="GetRequests"/>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet("/api/profile/requests")]
    [AccessTokenGuard]
    public async Task<PaginationResult<RequestDto>> GetPersonalRequests([FromQuery] SearchPersonalRequestRequest req)
    {
        var requests = await requestService.GetPersonalRequests(req);
        return mapper.Map<Request, RequestDto>(requests);
    }

    [HttpGet("{id}")]
    [AccessTokenGuard]
    public async Task<RequestDto> GetRequestById(Guid id)
    {
        var request = await requestService.GetRequestById(id);
        return mapper.Map<Request, RequestDto>(request);
    }

    [HttpPost]
    [AccessTokenGuard]
    public async Task<RequestDto> CreateRequest(CreateRequestDto dto)
    {
        var request = await requestService.CreateRequest(dto);
        return mapper.Map<Request, RequestDto>(request);
    }

    /// <summary>
    /// Admin updates request's Status or Reply.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<RequestDto> UpdateRequest(Guid id, UpdateRequestDto req)
    {
        var request = await requestService.UpdateRequest(id, req);
        return mapper.Map<Request, RequestDto>(request);
    }
}
