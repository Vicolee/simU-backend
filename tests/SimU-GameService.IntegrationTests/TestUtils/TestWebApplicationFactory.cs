using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using SimU_GameService.Api;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Api.Middleware;
using SimU_GameService.Application;
using SimU_GameService.Infrastructure.Persistence;

namespace SimU_GameService.IntegrationTests.TestUtils;

public class TestWebApplicationFactory<TEntryPoint>
    : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            var configuration = configurationBuilder
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "AgentService:BaseUri", Constants.AgentService.BaseUri },
                    { "Authentication:TokenUri", Constants.Authentication.TokenUri },
                    { "Authentication:ValidIssuer", Constants.Authentication.ValidIssuer },
                    { "Authentication:Audience", Constants.Authentication.Audience },
                    { "ConnectionStrings:SimUDbCloud", Constants.ConnectionStrings.ProdDBConnectionString },
                    { "ConnectionStrings:SimUDbDev", Constants.ConnectionStrings.DevDBConnectionString }
                }).Build();

            builder.ConfigureServices(services =>
            {
                services.AddWebAPI();
                services.AddApplication();
                services.AddInfrastructure(configuration);
            });

            builder.Configure(app =>
            {
                app.UseMiddleware<HubAuthenticationMiddleware>();
                app.UseAuthentication();
                app.UseAuthorization();                
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<UnityHub>("/unity");
                });
            });
        });
    }
}