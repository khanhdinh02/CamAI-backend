using AutoMapper;

namespace Infrastructure.Mapping.Profiles;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Core.Domain.Models.Publishers.ActivatedEdgeBoxMessage, Observer.Messages.ActivatedEdgeBoxMessage>();
    }
}