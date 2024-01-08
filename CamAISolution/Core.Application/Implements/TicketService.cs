using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
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

    private async Task CheckTechnician(Guid technicianId)
    {
        var account = await accountService.GetAccountById(technicianId);
        if (!account.HasRole(RoleEnum.Technician))
            throw new BadRequestException($"Account with {technicianId} is not technician");
        if (account.AccountStatusId == AccountStatusEnum.Inactive)
            throw new BadRequestException($"Account with {technicianId} is {nameof(AccountStatusEnum.Inactive)}");
    }

    private async Task CheckShop(Guid shopId)
    {
        var foundShop = await unitOfWork.GetRepository<Shop>().GetAsync(new ShopByIdRepoSpec(shopId));
        if (foundShop.IsValuesEmpty)
            throw new NotFoundException(typeof(Shop), shopId);
        if (foundShop.Values[0].ShopStatusId == ShopStatusEnum.Inactive)
            throw new BadRequestException($"{nameof(Shop)} was {nameof(ShopStatusEnum.Inactive)}");
    }

    private async Task CheckTicketType(int ticketTypeId)
    {
        if (!await unitOfWork.TicketTypes.IsExisted(ticketTypeId))
            throw new NotFoundException(typeof(TicketType), ticketTypeId);
    }

    private async Task CheckTicketStatus(int ticketStatusId)
    {
        if (!await unitOfWork.TicketStatuses.IsExisted(ticketStatusId))
            throw new NotFoundException(typeof(TicketStatus), ticketStatusId);
    }

    public async Task<Ticket> CreateTicket(CreateTicketDto ticketDto)
    {
        if (ticketDto.AssignedToId.HasValue)
            await CheckTechnician(ticketDto.AssignedToId.Value);
        if (ticketDto.ShopId.HasValue)
            await CheckShop(ticketDto.ShopId.Value);
        await CheckTicketType(ticketDto.TicketTypeId);
        var ticket = mapping.Map<CreateTicketDto, Ticket>(ticketDto);
        ticket.TicketStatusId = TicketStatusEnum.New;
        ticket = await unitOfWork.Tickets.AddAsync(ticket);
        logger.Info($"A new {nameof(Ticket)} with ID: {ticket.Id} was created");
        await unitOfWork.CompleteAsync();
        return ticket;
    }

    public async Task<Ticket> UpdateTicketStatus(Guid id, int statusId)
    {
        var ticket = await unitOfWork.Tickets.GetByIdAsync(id) ?? throw new NotFoundException(typeof(Ticket), statusId);
        await CheckTicketStatus(statusId);
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
        await CheckTicketType(ticketDto.TicketTypeId);
        if (ticketDto.AssignedToId.HasValue)
            await CheckTechnician(ticketDto.AssignedToId.Value);
        if (ticketDto.ShopId.HasValue)
            await CheckShop(ticketDto.ShopId.Value);
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
