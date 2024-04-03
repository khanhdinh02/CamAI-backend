using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Infrastructure.Observer.Messages;

namespace Infrastructure.Mapping.Profiles;

public class CameraProfile : Profile
{
    public CameraProfile()
    {
        CreateMap<Camera, CameraDto>();
        CreateMap<Camera, Camera>();
        CreateMap<Camera, EdgeBoxCameraDto>().ReverseMap();
    }
}
