using Core.Domain;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Host.CamAI.API;

public static class ApiApplicationBuilder
{
    public static IApplicationBuilder Migration(this IApplicationBuilder app, string[] args)
    {
        foreach (var arg in args)
        {
            Console.WriteLine(arg);
        }
        if (args.Contains("--run-migration"))
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<IAppLogging<Program>>();
            logger.Info("Applying migration");
            scope.ServiceProvider.GetService<CamAIContext>()?.Database.Migrate();
            logger.Info("Migration done");
        }
        return app;
    }
}
