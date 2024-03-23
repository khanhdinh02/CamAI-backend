using System.Reflection;
using Core.Domain.Interfaces.Services;
using Infrastructure.Observer.Messages;
using MassTransit;

namespace Infrastructure.Observer.Services;

public class MessageQueueService(IPublishEndpoint bus) : IMessageQueueService
{
    public Task Publish(MessageType type, object instanceValue)
    {
        var messageObject = GetMessageObject(type, instanceValue);
        return bus.Publish(messageObject);
    }

    private object GetMessageObject(MessageType type, object instanceValue)
    {
        return type switch
        {
            MessageType.ActivateEdgeBox => MapToMessageObject<ActivatedEdgeBoxMessage>(instanceValue),
            _ => throw new ArgumentException("No type is set")
        };
    }

    private T MapToMessageObject<T>(object instanceValue)
    {
        if (typeof(T).GetCustomAttribute<MessageUrnAttribute>() == null)
        {
            throw new ArgumentException(
                $"Class {nameof(T)} is not defined as a message. Please check whether class is annotated with [{nameof(MessageUrnAttribute)}]");
        }

        var resultObject = Activator.CreateInstance<T>();
        var fieldAndValueOfInstance = instanceValue
            .GetType()
            .GetProperties()
            .ToDictionary(
                src => src.Name,
                src => instanceValue.GetType().GetProperties().Single(x => x.Name == src.Name)
                    .GetValue(instanceValue, null)
            );

        foreach (var prop in resultObject!.GetType().GetProperties())
        {
            prop.SetValue(resultObject, fieldAndValueOfInstance[prop.Name], null);
        }

        return resultObject;
    }
}