using Core.Domain.Models.Configurations;
using Host.CamAI.API;
using Host.CamAI.API.Middlewares;
using Infrastructure.Jwt;
using Infrastructure.Logging;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args).ConfigureSerilog();
var configuration = builder.Configuration.Get<JwtConfiguration>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .Services
    .AddRepository(builder.Configuration.GetConnectionString("Default"))
    .AddJwtService(builder.Configuration)
    .AddLoggingDependencyInjection()
    .AddHttpContextAccessor()
    .AddServices();



var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/*
app.UseAuthentication();
app.UseAuthorization();*/

app.MapControllers();

app.Run();
