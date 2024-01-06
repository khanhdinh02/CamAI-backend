using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class TicketService(IUnitOfWork unitOfWork, IAppLogging<TicketService> logger, IBaseMapping mapping) : ITicketService
{

    public async Task<PaginationResult<Ticket>> SearchTicket(TicketSearchRequest searchRequest)
    {
        return await unitOfWork.Tickets.GetAsync(new TicketSearchSpec(searchRequest));
    }
    public async Task<Ticket> CreateTicket(CreateTicketDto ticketDto)
    {
        await ValidateCreateTicketDto(ticketDto);
        var ticket = mapping.Map<CreateTicketDto, Ticket>(ticketDto);
        ticket.TicketStatusId = TicketStatusEnum.New;
        ticket = await unitOfWork.Tickets.AddAsync(ticket);
        await unitOfWork.CompleteAsync();
        logger.Info($"{nameof(Ticket)} was created: {System.Text.Json.JsonSerializer.Serialize(ticket)}");
        return ticket;
    }

    public async Task<Ticket> UpdateTicketStatus(Guid id, int statusId)
    {
        var ticket = await unitOfWork.Tickets.GetByIdAsync(id);
        if(ticket == null)
            throw new NotFoundException(typeof(Ticket), statusId);
        var IsStatusExisted = await unitOfWork.TicketStatuses.IsExisted(statusId);
        if(!IsStatusExisted)
            throw new NotFoundException(typeof(TicketStatus), statusId);
        ticket.TicketStatusId = statusId;
        ticket = unitOfWork.Tickets.Update(ticket);
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