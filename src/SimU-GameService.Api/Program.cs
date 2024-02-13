using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using SimU_GameService.Api.Common;
using SimU_GameService.Api.Filters;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Application;
using SimU_GameService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// mapping
builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddSignalR(options =>
{
    options.AddFilter<ErrorHandlingHubFilter>();
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ErrorHandlingFilterAttribute>();
});

// Dependency injection by layer
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

app.UseAuthorization();

app.MapControllers();
app.MapHub<UnityHub>("/unity");

app.Run();
