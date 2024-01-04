using Core.Domain;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Host.CamAI.API;

public static class ApiApplicationBuilder
{
    public static IApplicationBuilder Migration(this IApplicationBuilder app, string[] args)
    {
        if (args.Contains(Core.Domain.Models.Configurations.AppStartUpArgumentConstant.Migration))
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<IAppLogging<Program>>();
            logger.Info("Applying migration");
            try
            {
                scope.ServiceProvider.GetService<CamAIContext>()?.Database.Migrate();
                logger.Info("Migration done");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        return app;
    }
}
