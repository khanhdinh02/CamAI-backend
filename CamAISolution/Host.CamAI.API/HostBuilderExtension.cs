using Serilog;

namespace Host.CamAI.API;

public static class HostBuilderExtension
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder
            .Host
            .UseSerilog(
                (context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext()
            );
        return builder;
    }
}
