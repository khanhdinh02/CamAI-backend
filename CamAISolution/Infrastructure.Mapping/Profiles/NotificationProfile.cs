using System.ComponentModel;
using AutoMapper;
using AutoMapper.Internal;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Entities.Base;
using Core.Domain.Models.DTO;

namespace Infrastructure.Mapping.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>()
            .ForMember(des => des.SentTo, member => member.MapFrom(src => src.SentTo.Select(an => an.Account)))
            .AfterMap((src, des) => des.SentBy.SentNotifications.Clear());
        CreateMap<LookupEntity, LookupDto>();
        CreateMap<AccountNotification, NotificationDto>()
            .ForMember(des => des.Status, member => member.Ignore())
            .ConstructUsing(
                (src, ctx) =>
                {
                    var result = ctx.Mapper.Map<Notification, NotificationDto>(src.Notification);
                    result.Status = ctx.Mapper.Map<LookupEntity, LookupDto>(src.Status);
                    return result;
                }
            );
        CreateMap<CreateNotificationDto, Notification>();
    }
}
