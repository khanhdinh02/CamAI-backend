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

    [HttpGet]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.Technician)]
    public async Task<ActionResult<PaginationResult<TicketDto>>> SearchTicket(TicketSearchRequest searchReq)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.SearchTicket(searchReq)));
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto ticketDto)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.CreateTicket(ticketDto)));
    }

    [HttpPatch("{id}/status")]
    [AccessTokenGuard(RoleEnum.Admin, RoleEnum.Technician)]
    public async Task<ActionResult<TicketDto>> UpdateTicketStatus(Guid id, int ticketStatusId)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.UpdateTicketStatus(id, ticketStatusId)));
    }
}
