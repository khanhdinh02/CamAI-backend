namespace Core.Domain.Interfaces.Services;

public interface IMessageQueueService
{
    Task Publish(MessageType type, object instanceValue);
}

public enum MessageType
{
    ConfirmedActivated
}