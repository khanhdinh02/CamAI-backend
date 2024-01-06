using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class TicketService(
    IUnitOfWork unitOfWork,
    IAppLogging<TicketService> logger,
    IBaseMapping mapping,
    IAccountService accountService
) : ITicketService
{
    public async Task<PaginationResult<Ticket>> SearchTicket(TicketSearchRequest searchRequest)
    {
        return await unitOfWork.Tickets.GetAsync(new TicketSearchSpec(searchRequest));
    }

    public async Task<Ticket> CreateTicket(CreateTicketDto ticketDto)
    {
        if (ticketDto.AssignedToId.HasValue)
        {
            var account = await accountService.GetAccountById(ticketDto.AssignedToId.Value);
            if (!account.HasRole(RoleEnum.Technician))
                throw new BadRequestException($"Account with {account.Id} is not technician");
            if (account.AccountStatusId == AccountStatusEnum.Inactive)
                throw new BadRequestException($"Account with {account.Id} is {nameof(AccountStatusEnum.Inactive)}");
        }
        if (ticketDto.ShopId.HasValue && !await unitOfWork.Shops.IsExisted(ticketDto.ShopId.Value))
            throw new NotFoundException(typeof(Shop), ticketDto.ShopId.Value);
        if (!await unitOfWork.TicketTypes.IsExisted(ticketDto.TicketTypeId))
            throw new NotFoundException(typeof(TicketType), ticketDto.TicketTypeId);
        var ticket = mapping.Map<CreateTicketDto, Ticket>(ticketDto);
        ticket.TicketStatusId = TicketStatusEnum.New;
        ticket = await unitOfWork.Tickets.AddAsync(ticket);
        logger.Info($"A new {nameof(Ticket)} with ID: {ticket.Id} was created");
        await unitOfWork.CompleteAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateTicketStatus(Guid id, int statusId)
    {
        var ticket = await unitOfWork.Tickets.GetByIdAsync(id);
        if (ticket == null)
            throw new NotFoundException(typeof(Ticket), statusId);
        var IsStatusExisted = await unitOfWork.TicketStatuses.IsExisted(statusId);
        if (!IsStatusExisted)
            throw new NotFoundException(typeof(TicketStatus), statusId);
        ticket.TicketStatusId = statusId;
        ticket = unitOfWork.Tickets.Update(ticket);
        await unitOfWork.CompleteAsync();
        return ticket;
    }

    public async Task<Ticket> GetTicketById(Guid id)
    {
        var ticket = await unitOfWork.Tickets.GetAsync(new TicketByIdRepoSpec(id));
        if (ticket.IsValuesEmpty)
            throw new NotFoundException(typeof(Ticket), id);
        return ticket.Values[0];
    }

    public async Task<Ticket> UpdateTicket(Guid id, UpdateTicketDto ticketDto)
    {
        var ticket = await GetTicketById(id);
        if (!await unitOfWork.TicketTypes.IsExisted(ticketDto.TicketTypeId))
            throw new NotFoundException(typeof(TicketType), ticketDto.TicketTypeId);
        if (ticketDto.AssignedToId.HasValue)
        {
            var account = await accountService.GetAccountById(ticketDto.AssignedToId.Value);
            if (!account.HasRole(RoleEnum.Technician))
                throw new BadRequestException($"Account with {account.Id} is not technician");
            if (account.AccountStatusId == AccountStatusEnum.Inactive)
                throw new BadRequestException($"Account with {account.Id} is {nameof(AccountStatusEnum.Inactive)}");
        }
        if (ticketDto.ShopId.HasValue && !await unitOfWork.Shops.IsExisted(ticketDto.ShopId.Value))
            throw new NotFoundException(typeof(Shop), ticketDto.ShopId.Value);
        ticket = unitOfWork.Tickets.Update(mapping.Map(ticketDto, ticket));
        await unitOfWork.CompleteAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateTicketReply(Guid id, UpdateTicketReplyDto tickeReplyDto)
    {
        var ticket = await GetTicketById(id);
        ticket = unitOfWork.Tickets.Update(mapping.Map(tickeReplyDto, ticket));
        await unitOfWork.CompleteAsync();
        return ticket;
    }

    //TODO [Dat]: More business and validation for creatinf and updating ticket
}
