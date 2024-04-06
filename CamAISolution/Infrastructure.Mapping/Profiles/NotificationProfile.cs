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
            .ForMember(des => des.Status, opts => opts.Ignore())
            .ConstructUsing(
                (src, ctx) =>
                {
                    var result = ctx.Mapper.Map<Notification, NotificationDto>(src.Notification);
                    result.Status = src.Status;
                    return result;
                }
            );
        CreateMap<CreateNotificationDto, Notification>();
    }
}
