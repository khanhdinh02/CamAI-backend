using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class IncidentProfile : Profile
{
    public IncidentProfile()
    {
        CreateMap<CreateIncidentDto, Incident>();
        CreateMap<Incident, IncidentDto>();
    }
}
