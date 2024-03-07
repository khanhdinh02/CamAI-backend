using AutoMapper;
using Core.Domain.Entities;
using Core.Domain.Models.DTO.Evidences;

namespace Infrastructure.Mapping.Profiles;

public class IncidentProfile : Profile
{
    public IncidentProfile()
    {
        CreateMap<CreateEvidenceDto, Evidence>();
    }
}