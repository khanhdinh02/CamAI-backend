using Host.CamAI.API;
using Host.CamAI.API.Middlewares;
using Host.CamAI.API.Models;
using Infrastructure.Jwt;
using Infrastructure.Logging;
using Infrastructure.Mapping;
using Infrastructure.Observer;
using Infrastructure.Notification;
using Infrastructure.Notification.Models;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args).ConfigureSerilog();

builder.Services.AddControllers();
const string allowPolicy = "AllowAll";

builder
    .Services.AddCors(
        opts =>
            opts.AddPolicy(
                name: allowPolicy,
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders(HeaderNameConstant.Auto)
            // TODO[Dat]: Enable allow credential when have specific origin
            // .AllowCredentials()
            )
    )
    .AddRepository(builder.Configuration.GetConnectionString("Default"))
    .AddJwtService(builder.Configuration)
    .AddLoggingDependencyInjection()
    .AddHttpContextAccessor()
    .AddSwagger()
    .AddServices()
    .AddMapping()
    .AddObserver()
    .AddNotification(builder.Configuration.GetRequiredSection("GoogleSecret").Get<GoogleSecret>());

builder.ConfigureMassTransit();

builder.Services.Configure<RouteOptions>(opts =>
{
    opts.LowercaseUrls = true;
    opts.LowercaseQueryStrings = true;
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseCors(allowPolicy);

app.UseMiddleware<GlobalJwtHandler>();

app.Migration(args);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

var observer = app.Services.GetRequiredService<SyncObserver>();
observer.RegisterEvent();

app.Run();
