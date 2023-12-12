using Host.CamAI.API;
using Host.CamAI.API.Middlewares;
using Infrastructure.Jwt;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args).ConfigureGrace().ConfigureSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepository(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddJwtService(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
