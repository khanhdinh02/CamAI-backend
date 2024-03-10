using System.Reflection;
using MassTransit;
using MassTransit.Configuration;

namespace Infrastructure.MessageQueue;

public static class ReceiveEndpointConfigurator
{
    private static readonly MethodInfo? MessageTopologyMethod = typeof(IMessageTopologyConfigurator).GetMethod(
        nameof(IMessageTopologyConfigurator.GetMessageTopology)
    );

    public static void RegisterPublisher(this IRabbitMqBusFactoryConfigurator cfg, IList<Assembly> assemblies)
    {
        var publishers = assemblies.GetEndpoints<PublisherAttribute>();
        foreach (var (publisher, endpoint) in publishers)
        {
            cfg.ChangeExchangeName(publisher, endpoint);

            cfg.Publish(
                publisher,
                x =>
                {
                    x.Exclude = true;
                }
            );
        }
    }

    private static void ChangeExchangeName(
        this IRabbitMqBusFactoryConfigurator cfg,
        Type publisher,
        PublisherAttribute endpoint
    )
    {
        var topology = cfg.MessageTopology;
        var topologyConfigurator = MessageTopologyMethod!.MakeGenericMethod(publisher).Invoke(topology, null);
        var method = topologyConfigurator!
            .GetType()
            .GetMethod(nameof(IMessageTopologyConfigurator<object>.SetEntityName));
        method!.Invoke(topologyConfigurator, [endpoint.QueueName]);
    }

    public static void RegisterConsumer(
        this IReceiveConfigurator cfg,
        IRegistrationContext context,
        IList<Assembly> assemblies
    )
    {
        var consumers = assemblies.GetEndpoints<ConsumerAttribute>().GroupBy(x => x.Endpoint.QueueName);
        if (cfg is IRabbitMqBusFactoryConfigurator rabbitMqCfg)
            foreach (var queue in consumers)
                RegisterConsumer(rabbitMqCfg, context, queue.Key, queue.Select(x => (x.Type, x.Endpoint)));
        else if (cfg is IInMemoryBusFactoryConfigurator)
            foreach (var topic in consumers)
                RegisterConsumer(cfg, context, topic.Key, topic.Select(x => x.Type));
    }

    private static IEnumerable<(Type Type, T Endpoint)> GetEndpoints<T>(this IEnumerable<Assembly> assemblies)
        where T : MessageQueueEndpointAttribute
    {
        var types =
            from asm in assemblies
            from type in asm.ExportedTypes
            let attr = type.GetCustomAttribute<T>()
            where attr != null
            select (type, attr);
        return types;
    }

    private static void RegisterConsumer(
        IRabbitMqBusFactoryConfigurator cfg,
        IRegistrationContext context,
        string queueName,
        IEnumerable<(Type Type, ConsumerAttribute consumer)> consumers
    )
    {
        cfg.ReceiveEndpoint(
            queueName,
            e =>
            {
                e.ConcurrentMessageLimit = 5;
                e.ConfigureConsumeTopology = false;
                e.DiscardFaultedMessages();
                e.DiscardSkippedMessages();

                foreach (var (type, attr) in consumers)
                {
                    e.ConfigureConsumer(context, type);
                    e.Bind(
                        attr.ExchangeName,
                        x =>
                        {
                            x.ExchangeType = attr.ExchangeType;
                            x.RoutingKey = attr.RoutingKey;
                        }
                    );
                }
            }
        );
    }

    private static void RegisterConsumer(
        IReceiveConfigurator cfg,
        IRegistrationContext context,
        string queueName,
        IEnumerable<Type> consumers
    )
    {
        cfg.ReceiveEndpoint(
            queueName,
            e =>
            {
                e.ConcurrentMessageLimit = 5;
                e.ConfigureConsumeTopology = false;
                e.DiscardFaultedMessages();
                e.DiscardSkippedMessages();

                foreach (var type in consumers)
                    e.ConfigureConsumer(context, type);
            }
        );
    }
}
