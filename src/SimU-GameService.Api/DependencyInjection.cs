using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.Common;
using SimU_GameService.Api.Middleware;
using SimU_GameService.Api.OnlineStatus;
using SimU_GameService.Infrastructure.Characters;

namespace SimU_GameService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
        services.AddSignalR(options =>
        {
            options.AddFilter<ErrorHandlingHubFilter>();
        });
        services.AddControllers(options =>
        {
            options.Filters.Add<ErrorHandlingFilterAttribute>();
        });
            
        services.AddSingleton<ConnectionService>();
        services.AddHostedService<OnlineStatusService>();
        services.AddHostedService<ConversationStatusService>();
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
