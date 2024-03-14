using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class CameraProfile : Profile
{
    public CameraProfile()
    {
        CreateMap<Camera, CameraDto>();
    }
}
