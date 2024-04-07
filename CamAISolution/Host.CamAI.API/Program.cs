using Core.Application.Events;
using Core.Application.Specifications;
using Core.Domain.Constants;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Host.CamAI.API;
using Host.CamAI.API.Middlewares;
using Host.CamAI.API.Models;
using Infrastructure.Cache;
using Infrastructure.Email;
using Infrastructure.Jwt;
using Infrastructure.Logging;
using Infrastructure.Mapping;
using Infrastructure.Observer;
using Infrastructure.Repositories;
using Infrastructure.Streaming;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args).ConfigureSerilog();

builder.Services.AddControllers();
const string allowPolicy = "AllowAll";

builder
    .Services.AddCors(opts =>
        opts.AddPolicy(
            allowPolicy,
            builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders(HeaderNameConstant.Auto)
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
    .AddBackgroundService()
    .AddCacheService()
    .AddEmailService(builder.Configuration)
    .AddEventListener()
    .AddStreaming(builder.Configuration)
    .AddBackgroundService();

builder.Services.AddHttpClient();

builder.ConfigureMassTransit();

builder.Services.Configure<RouteOptions>(opts =>
{
    opts.LowercaseUrls = true;
    opts.LowercaseQueryStrings = true;
});

var app = builder.Build();

app.UseWebSockets();
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

app.UseMiddleware<NotificationSocket>();

app.UseHttpsRedirection();

app.MapControllers();

RegisterSyncObserver();
AttachHumanCountFileSave();
InitiateEssentialCacheData(app.Services).GetAwaiter().GetResult();

app.MapGet("/", () => "Hello word");

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

async Task InitiateEssentialCacheData(IServiceProvider provider)
{
    using var scope = provider.CreateScope();
    var accRepo = scope.ServiceProvider.GetRequiredService<IRepository<Account>>();
    var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
    var admin = (await accRepo.GetAsync(new AccountByRoleSpec(Role.Admin).GetExpression())).Values[0];
    cache.Set(
        ApplicationCacheKey.Admin,
        admin,
        new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove }
    );
}
