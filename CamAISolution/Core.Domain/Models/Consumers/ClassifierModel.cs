namespace Core.Domain.Models.Consumers;

public class ClassifierModel
{
    public DateTime Time { get; set; }
    public List<ClassifierResult> Results { get; set; } = null!;
    public int Total { get; set; }
    public Guid ShopId { get; set; }
}

public class ClassifierResult
{
    public string ActionType { get; set; } = null!;
    public int Count { get; set; }
}

public static class ActionType
{
    public const string Idle = "idle";
    public const string Walking = "walking";
    public const string Phone = "phone";
    public const string Laptop = "laptop";
}
