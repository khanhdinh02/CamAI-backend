using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class NotificationDto : BaseDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public AccountDto SentBy { get; set; } = null!;
    public NotificationStatus Status { get; set; }
    public ICollection<AccountDto> SentTo { get; set; } = new HashSet<AccountDto>();
}
