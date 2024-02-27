using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.Common;
using SimU_GameService.Api.Middleware;

namespace SimU_GameService.Application;

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

        return services;
    }
}
