using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        // CreateMap<CreateImageDto, Image>().ForSourceMember(src => src.ImageBytes, opts => opts.DoNotValidate());
    }
}
