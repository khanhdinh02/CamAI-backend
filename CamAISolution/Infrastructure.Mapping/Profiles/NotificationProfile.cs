using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Infrastructure.Mapping.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>()
            .ForMember(
                x => x.EntityName,
                opts =>
                    opts.MapFrom(
                        (notification, _) =>
                        {
                            return notification.Type switch
                            {
                                NotificationType.EdgeBoxUnhealthy => nameof(EdgeBoxInstall),
                                _ => ""
                            };
                        }
                    )
            );
        CreateMap<AccountNotification, NotificationDto>()
            .ConstructUsing(
                (src, ctx) =>
                {
                    var result = ctx.Mapper.Map<Notification, NotificationDto>(src.Notification);
                    // TODO: how about status
                    return result;
                }
            );
        CreateMap<CreateNotificationDto, Notification>();
    }
}
