using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimU_GameService.Application.Common;

namespace SimU_GameService.Infrastructure.Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        /// add DB context for EF Core + Postgres SQL
        services.AddDbContext<SimUDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SimUDb")));

        // add repository abstractions
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
	}
}