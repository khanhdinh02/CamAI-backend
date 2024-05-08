using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class SupervisorAssignmentProfile : Profile
{
    public SupervisorAssignmentProfile()
    {
        CreateMap<SupervisorAssignment, SupervisorAssignmentDto>();
    }
}
