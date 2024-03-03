using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class NotificationDto : BaseDto
{
    public NotificationDto()
    {
        SentTo = new HashSet<AccountDtoWithBrand>();
    }

    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public AccountDtoWithBrand SentBy { get; set; } = null!;
    public NotificationStatus Status { get; set; }
    public ICollection<AccountDtoWithBrand> SentTo { get; set; }
}
