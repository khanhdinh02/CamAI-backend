using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController(ITicketService ticketService, IBaseMapping mapping) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.Technician)]
    public async Task<ActionResult<TicketDto>> GetTicketById(Guid id)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.GetTicketById(id)));
    }

    [HttpGet]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.Technician)]
    public async Task<ActionResult<PaginationResult<TicketDto>>> SearchTicket([FromQuery] TicketSearchRequest searchReq)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.SearchTicket(searchReq)));
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto ticketDto)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.CreateTicket(ticketDto)));
    }

    [HttpPatch("{id}/status/{ticketStatusId}")]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.Technician)]
    public async Task<ActionResult<TicketDto>> UpdateTicketStatus(Guid id, int ticketStatusId)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.UpdateTicketStatus(id, ticketStatusId)));
    }

    /// <summary>
    /// Admin update ticket information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ticketDto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<TicketDto>> UpdateTicket(Guid id, UpdateTicketDto ticketDto)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.UpdateTicket(id, ticketDto)));
    }

    [HttpPut("{id}/reply")]
    [AccessTokenGuard(RoleEnum.Technician)]
    public async Task<ActionResult<TicketDto>> UpdateTicketReply(Guid id, UpdateTicketReplyDto ticketReplyDto)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.UpdateTicketReply(id, ticketReplyDto)));
    }
}
