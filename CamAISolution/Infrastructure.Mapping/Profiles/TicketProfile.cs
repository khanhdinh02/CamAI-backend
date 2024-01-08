using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<Ticket, TicketDto>();
        CreateMap<CreateTicketDto, Ticket>();
        CreateMap<UpdateTicketDto, Ticket>();
        CreateMap<UpdateTicketReplyDto, Ticket>();
        CreateMap<TicketType, LookupDto>();
        CreateMap<TicketStatus, LookupDto>();
    }
}
