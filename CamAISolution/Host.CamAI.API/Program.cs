using Host.CamAI.API;
using Host.CamAI.API.Middlewares;
using Infrastructure.Jwt;
using Infrastructure.Logging;
using Infrastructure.Mapping;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args).ConfigureSerilog();

builder.Services.AddControllers();

builder
    .Services
    .AddRepository(builder.Configuration.GetConnectionString("Default"))
    .AddJwtService(builder.Configuration)
    .AddLoggingDependencyInjection()
    .AddHttpContextAccessor()
    .AddSwagger()
    .AddServices()
    .AddMapping();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

app.Migration();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
