using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
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
