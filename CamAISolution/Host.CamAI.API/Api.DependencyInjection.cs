using Core.Application.Implements;
using Core.Domain.Services;
using Microsoft.OpenApi.Models;

namespace Host.CamAI.API;

public static class ApiDependencyInjection
{
    public const string AllowPolicy = "AllowAll";
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IShopService, ShopService>();
        services.AddCors(opts => opts.AddPolicy(
            name: AllowPolicy,
            builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyHeader()
            // TODO [Dat]: Enable allow credential when have specific origin
            // .AllowCredentials()
        ));
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
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
        });
        return services;
    }
}
