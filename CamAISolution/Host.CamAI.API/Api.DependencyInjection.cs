using System.Reflection;
using System.Text.Json.Serialization;
using Core.Application.Events;
using Core.Application.Implements;
using Core.Domain.Interfaces.Services;
using Core.Domain.Services;
using Microsoft.OpenApi.Models;

namespace Host.CamAI.API;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IShopService, ShopService>();
        services.AddScoped<IEdgeBoxService, EdgeBoxService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddSingleton<EventManager>();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "CamAI API", Version = "v1" });
            option.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }
            );
            option.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                }
            );
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            option.IncludeXmlComments(xmlPath);
        });
        return services;
    }
}
