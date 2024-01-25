using System.Reflection;
using Host.CamAI.API.Models.Messages;
using Infrastructure.MessageQueue;
using MassTransit;

namespace Host.CamAI.API;

public static class MassTransitConfiguration
{
    public static void ConfigureMassTransit(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSection(RabbitMqConfiguration.Section).Get<RabbitMqConfiguration>();
        var assemblies = new[] { typeof(Program).Assembly };

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumers(assemblies);

            if (settings!.HostName == "in-memory")
            {
                RegisterPublisherEndpoint(assemblies);
                x.UsingInMemory((context, cfg) => cfg.RegisterConsumer(context, assemblies));
            }
            else
                x.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        cfg.SetupHost(settings);

                        // TODO [Duy]: Cannot set the routing yet using reflection yet so we have to declare by hand
                        cfg.Send<TestMessage>(configurator =>
                            configurator.UseRoutingKeyFormatter(sendContext => sendContext.Message.RoutingKey)
                        );

                        cfg.RegisterPublisher(assemblies);
                        cfg.RegisterConsumer(context, assemblies);
                    }
                );
        });
    }

    private static void RegisterPublisherEndpoint(Assembly[] assemblies)
    {
        var mapMethod = typeof(EndpointConvention).GetMethod(nameof(EndpointConvention.Map), [typeof(Uri)])!;
        var typesToPublish = GetEntityTypeAndBusEndpoint(assemblies);
        foreach (var (type, uri) in typesToPublish)
            mapMethod.MakeGenericMethod(type).Invoke(null, [uri]);
    }

    private static IEnumerable<(Type type, Uri Uri)> GetEntityTypeAndBusEndpoint(IEnumerable<Assembly> loadedAssemblies)
    {
        return from asm in loadedAssemblies
            from type in asm.ExportedTypes
            let attr = type.GetCustomAttribute<PublisherAttribute>()
            where attr != null
            select (type, attr.Uri);
    }

    private static void SetupHost(this IRabbitMqBusFactoryConfigurator cfg, RabbitMqConfiguration settings)
    {
        cfg.Host(
            settings.HostName,
            settings.Port,
            settings.VirtualHost,
            h =>
            {
                h.Username(settings.Username);
                h.Password(settings.Password);
            }
        );
    }
}
