namespace Core.Domain.Interfaces.Services;

public interface IMessageQueueService
{
    Task Publish(object messageObject);
}