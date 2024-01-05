using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.Models.DTO;

namespace Infrastructure.Mapping.Profiles;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<Ticket, TicketDto>();
        CreateMap<CreateTicketDto, Ticket>();
    }
}
