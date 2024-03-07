using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class EvidenceProfile : Profile
{
    public EvidenceProfile()
    {
        CreateMap<CreateIncidentDto, Incident>();
    }
}