using SimU_GameService.Api.Filters;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Application;
using SimU_GameService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ErrorHandlingFilterAttribute>();
});

// Dependency injection by layer
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<UnityHub>("/unity");

app.Run();
