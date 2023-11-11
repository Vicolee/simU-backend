using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using SimU_GameService.Application.Common.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimU_GameService.Infrastructure.Persistence.Repositories;

namespace SimU_GameService.Infrastructure.Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        /// add DB context for EF Core + Postgres SQL
        services.AddDbContext<SimUDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SimUDbCloud")));

        // add repository abstractions
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        


        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("firebase.json")
        });

        // changed from AddSingleton to AddScoped and the migration worked. Check back on this
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        // To Do: Make sure I added this service correctly, and figure out how to add HTTP client for this service
        services.AddScoped<ILLMService, LLMService>();
        services.Configure<AuthenticationSettings>(configuration.GetSection("Authentication"));

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((sp, httpClient) =>
        {
            var authSettings = sp.GetRequiredService<IOptions<AuthenticationSettings>>().Value;
            httpClient.BaseAddress = new Uri(authSettings.TokenUri ?? string.Empty);
        });

        services.AddHttpClient<ILLMService, LLMService>( httpClient =>
        {
            // TO DO: UPDATE THIS LLM SERVICE URL
            httpClient.BaseAddress = new Uri("https://LLM-service-url.com");
        });

        services
            .AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
            {
                jwtOptions.Authority = configuration["Authentication:ValidIssuer"];
                jwtOptions.Audience = configuration["Authentication:Audience"];
                jwtOptions.TokenValidationParameters.ValidIssuer =
                    configuration["Authentication:ValidIssuer"];
            });

        return services;
	}
}