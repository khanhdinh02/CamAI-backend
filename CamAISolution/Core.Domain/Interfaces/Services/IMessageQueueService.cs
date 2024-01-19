namespace Core.Domain.Interfaces.Services;

public interface IMessageQueueService
{
    Task SendMessage<T>(T message, string sendToQueue, string exchange);
}
