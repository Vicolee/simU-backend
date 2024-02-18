﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimU_GameService.Infrastructure.Persistence.Repositories;
using SimU_GameService.Infrastructure.Authentication;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Infrastructure.Characters;

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
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IWorldRepository, WorldRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IResponseRepository, ResponseRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IAgentRepository, AgentRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.Decorate<ILocationRepository, CachedLocationRepository>();

        // AI agent service
        services.AddScoped<ILLMService, LLMService>();
        services.AddHttpClient<ILLMService, LLMService>((sp, httpClient) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var baseUri = configuration["AgentService:BaseUri"]
                ?? throw new NotFoundException("AgentService:BaseUri not specified in appsettings.json");
            httpClient.BaseAddress = new Uri(baseUri);
        });

        // Online status service checker - runs in background at all times
        // GET HELP FROM LEKINA ON THIS - WHAT SHOULD THE URI BE FOR THIS??
        services.AddHostedService<OnlineStatusService>();
        services.AddHttpClient<IOnlineStatusService, OnlineStatusService>((sp, httpClient) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var baseUri = configuration["OnlineStatusService:BaseUri"]
                ?? throw new NotFoundException("OnlineStatusService:BaseUri not specified in appsettings.json");
            httpClient.BaseAddress = new Uri(baseUri);
        });

        // service that checks if ongoing conversations have had recent activity.
        // it checks every 15 minutes.
        services.AddHostedService<ConversationStatusService>();

        // authentication
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("firebase.json")
        });

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddHttpClient<IAuthenticationService, AuthenticationService>((sp, httpClient) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var tokenUri = configuration["Authentication:TokenUri"]
                ?? throw new NotFoundException("Authentication:TokenUri not specified in appsettings.json");
            httpClient.BaseAddress = new Uri(tokenUri);
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