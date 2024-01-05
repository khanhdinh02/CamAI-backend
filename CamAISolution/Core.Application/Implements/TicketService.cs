using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO.Tickets;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.DTO;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class TicketService(IUnitOfWork unitOfWork, IAppLogging<TicketService> logger, IBaseMapping mapping) : ITicketService
{
    public async Task<Ticket> CreateTicket(CreateTicketDto ticketDto)
    {
        await ValidateCreateTicketDto(ticketDto);
        var ticket = mapping.Map<CreateTicketDto, Ticket>(ticketDto);
        ticket.TicketStatusId = TicketStatusEnum.Open;
        ticket = await unitOfWork.Tickets.AddAsync(ticket);
        await unitOfWork.CompleteAsync();
        return ticket;
    }

    private async Task ValidateCreateTicketDto(CreateTicketDto ticketDto)
    {
        if (ticketDto.AssignedToId.HasValue && !await unitOfWork.Accounts.IsExisted(ticketDto.AssignedToId.Value))
            throw new NotFoundException(typeof(Account), ticketDto.AssignedToId.Value);
        if (ticketDto.ShopId.HasValue && !await unitOfWork.Shops.IsExisted(ticketDto.ShopId.Value))
            throw new NotFoundException(typeof(Shop), ticketDto.ShopId.Value);
        if (!await unitOfWork.TicketTypes.IsExisted(ticketDto.TicketTypeId))
            throw new NotFoundException(typeof(TicketType), ticketDto.TicketTypeId);
    }
}
