using System.Reflection;
using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models.Publishers;
using Infrastructure.MessageQueue;
using Infrastructure.Observer.Messages;
using MassTransit;

namespace Infrastructure.Observer.Services;

public class MessageQueueService(IPublishEndpoint bus, IBaseMapping mapping, IAppLogging<MessageQueueService> logging)
    : IMessageQueueService
{
    private static readonly Dictionary<string, Type> messageTypes = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsClass && t.Namespace == typeof(PublisherMessage).Namespace)
        .ToDictionary(t => t.Name, t => t);

    public Task Publish(object messageObject) => bus.Publish(GetPublisherMessageObject(messageObject));

    private object GetPublisherMessageObject(object messageObject)
    {
        if (messageTypes.TryGetValue(messageObject.GetType().Name, out var publisherType))
        {
            var result = mapping.Map(messageObject, Activator.CreateInstance(publisherType))!;
            return result;
        }
        logging.Error("Mapping is failed");
        throw new ServiceUnavailableException("Service unavailable");
    }
}
