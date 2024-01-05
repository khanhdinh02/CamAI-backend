using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController(ITicketService ticketService, IBaseMapping mapping) : ControllerBase
{
    [HttpPost]
    [AccessTokenGuard(RoleEnum.Admin)]
    public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto ticketDto)
    {
        return Ok(mapping.Map<Ticket, TicketDto>(await ticketService.CreateTicket(ticketDto)));
    }
}
