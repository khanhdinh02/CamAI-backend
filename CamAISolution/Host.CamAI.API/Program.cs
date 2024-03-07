using Core.Application.Events;
using Host.CamAI.API;
using Host.CamAI.API.Middlewares;
using Host.CamAI.API.Models;
using Infrastructure.Jwt;
using Infrastructure.Logging;
using Infrastructure.Mapping;
using Infrastructure.Notification;
using Infrastructure.Notification.Models;
using Infrastructure.Observer;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args).ConfigureSerilog();

builder.Services.AddControllers();
const string allowPolicy = "AllowAll";

builder
    .Services.AddCors(opts =>
        opts.AddPolicy(
            name: allowPolicy,
            builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders(HeaderNameConstant.Auto)
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
    .AddServices(builder.Configuration)
    .AddMapping()
    .AddObserver(builder.Configuration)
    .AddNotification(builder.Configuration.GetRequiredSection("GoogleSecret").Get<GoogleSecret>())
    .AddBackgroundService();

builder.Services.AddHttpClient();

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
    app.UseReDoc(config =>
    {
        config.DocumentTitle = "CamAI API Documentation";
        config.SpecUrl = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseWebSockets();

RegisterSyncObserver();
AttachHumanCountFileSave();

app.Run();
return;

void RegisterSyncObserver()
{
    var observer = app.Services.GetRequiredService<SyncObserver>();
    observer.RegisterEvent();
}

void AttachHumanCountFileSave()
{
    var humanCount = app.Services.GetRequiredService<HumanCountFileSaverObserver>();
    var subject = app.Services.GetRequiredService<HumanCountSubject>();
    subject.Attach(humanCount);
}
