namespace Core.Domain.Models.Consumers;

public class HumanCountModel
{
    public DateTime Time { get; set; }
    public int Total { get; set; }
    public Guid ShopId { get; set; }
}

public static class ActionType
{
    public const string Idle = "idle";
    public const string Walking = "walking";
    public const string Phone = "phone";
    public const string Laptop = "laptop";
}
