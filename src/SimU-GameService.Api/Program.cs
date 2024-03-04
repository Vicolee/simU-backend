using Microsoft.OpenApi.Models;
using SimU_GameService.Api;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Application;
using SimU_GameService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Dependency injection by layer
builder.Services.AddWebAPI();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SimU-GameService API",
        Version = "v1"
    });
    options.AddSignalRSwaggerGen();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<UnityHub>("/unity");

app.Run();

public partial class Program { }