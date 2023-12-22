using Host.CamAI.API;
using Host.CamAI.API.Middlewares;
using Infrastructure.Jwt;
using Infrastructure.Logging;
using Infrastructure.Repositories;
using Infrastructure.Mapping;

var builder = WebApplication.CreateBuilder(args).ConfigureSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .Services
    .AddRepository(builder.Configuration.GetConnectionString("Default"))
    .AddJwtService(builder.Configuration)
    .AddLoggingDependencyInjection()
    .AddHttpContextAccessor()
    .AddServices()
    .AddMapping();

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
