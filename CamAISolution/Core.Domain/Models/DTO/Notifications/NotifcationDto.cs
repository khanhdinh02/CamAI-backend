namespace Core.Domain.DTO;

public class NotifcationDto : BaseDto
{
    public NotifcationDto()
    {
        SentTo = new HashSet<AccountDto>();
    }

    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public AccountDto SentBy { get; set; } = null!;
    public LookupDto Status { get; set; } = null!;

    public ICollection<AccountDto> SentTo { get; set; }
}
