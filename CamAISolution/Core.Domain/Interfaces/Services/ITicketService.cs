using Core.Domain.Entities;
using Core.Domain.Models.DTO;

namespace Core.Domain.Interfaces.Services;

public interface ITicketService
{
    Task<Ticket> CreateTicket(CreateTicketDto ticketDto);
}
