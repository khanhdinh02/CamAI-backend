using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace Host.CamAI.API;

public static class ApiApplicationBuilder
{
    public static IApplicationBuilder Migration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        scope.ServiceProvider.GetService<CamAIContext>()?.Database.Migrate();
        return app;
    }
}
