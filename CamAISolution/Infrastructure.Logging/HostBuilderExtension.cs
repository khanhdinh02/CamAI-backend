using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Infrastructure.Logging;

public static class HostBuilderExtension
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(
            (context, logConfig) => logConfig.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext()
        );
        return builder;
    }
}
