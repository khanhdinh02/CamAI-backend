using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<CreateRequestDto, Request>();
        CreateMap<Request, RequestDto>();
    }
}
