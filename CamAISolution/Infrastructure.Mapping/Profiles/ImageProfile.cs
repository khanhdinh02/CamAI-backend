using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.DTO;

namespace Infrastructure.Mapping.Profiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDto>();
    }
}
