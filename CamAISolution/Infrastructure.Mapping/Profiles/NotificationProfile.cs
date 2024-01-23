using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Models.DTO;

namespace Infrastructure.Mapping.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>()
            .ForMember(des => des.SentTo, member => member.MapFrom(src => src.SentTo.Select(an => an.Account)))
            .AfterMap((src, des) => des.SentBy.SentNotifications.Clear());
        CreateMap<CreateNotificationDto, Notification>();
    }
}
