using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Services;

public interface ITicketService
{
    Task<Ticket> CreateTicket(CreateTicketDto ticketDto);
    Task<Ticket> UpdateTicketStatus(Guid id, int statusId);
    Task<PaginationResult<Ticket>> SearchTicket(TicketSearchRequest searchRequest);
    Task<Ticket> GetTicketById(Guid id);
    Task<Ticket> UpdateTicket(Guid id, UpdateTicketDto ticketDto);
    Task<Ticket> UpdateTicketReply(Guid id, UpdateTicketReplyDto tickeReplyDto);
}
