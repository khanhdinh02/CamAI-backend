using System.Reflection;
using Grace.AspNetCore.Hosting;
using Grace.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
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

    public static WebApplicationBuilder ConfigureGrace(this WebApplicationBuilder builder)
    {
        var loadedAssemblies = LoadModules();
        builder
            .Host
            .UseGrace()
            .ConfigureContainer<IInjectionScope>(
                (_, scope) =>
                {
                    scope.Configure(config =>
                    {
                        config.Export<ServiceProviderIsServiceImpl>().As<IServiceProviderIsService>();
                        config.ExportAssemblies(loadedAssemblies).ExportAttributedTypes();
                    });
                }
            );
        return builder;
    }

    private static List<Assembly> LoadModules()
    {
        // load all dll
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var matcher = new Matcher();
        matcher.AddInclude("*.dll");
        return matcher.GetResultsInFullPath(path).Select(Assembly.LoadFrom).ToList();
    }

    private sealed class ServiceProviderIsServiceImpl(IExportLocatorScope locatorScope) : IServiceProviderIsService
    {
        public bool IsService(Type serviceType) =>
            !serviceType.IsGenericTypeDefinition && locatorScope.CanLocate(serviceType);
    }
}
