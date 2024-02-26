using Microsoft.AspNetCore.SignalR.Client;
using SimU_GameService.Domain.Models;
using SimU_GameService.IntegrationTests.TestUtils;

namespace SimU_GameService.IntegrationTests.Hubs;

public class SignalREndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private HubConnection _connection = default!;
    private readonly TestWebApplicationFactory<Program> _factory;

    public SignalREndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _factory = factory;
        _client = _factory.CreateClient();
    }

    private static HubConnection InitializeHubConnection(string authToken, string hubURL)
        => new HubConnectionBuilder()
            .WithUrl($"{hubURL}?access_token={authToken}")
            .Build();

    private static async Task<HubConnection> StartConnectionAsync(
        HttpMessageHandler handler, string hubURL, string authToken)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{hubURL}?access_token={authToken}", o =>
            {
                o.HttpMessageHandlerFactory = _ => handler;
            })
            .Build();

        await hubConnection.StartAsync();    
        return hubConnection;
    }
            

    [Fact]
    public async void UpdateLocation_WhenUserExists_ShouldReturnOk()
    {
        // arrange        
        // register user to get ID and auth token
        var registerResponse = await TestUserUtils.RegisterUser(
            _client,
            Constants.User.TestUsername,
            Constants.User.TestEmail,
            Constants.User.TestPassword);

        var userId = registerResponse!.Id;
        var authToken = registerResponse!.AuthToken;

        // generate random location
        Random random = new();
        var location = new Location(random.Next(0, 100), random.Next(0, 100));

        // act
        // initialize SignalR connection
        var hubURL = new Uri($"{Constants.Routes.BaseUri}{Constants.Routes.UnityHub.Route}");
        _connection = await StartConnectionAsync(_factory.Server.CreateHandler(), hubURL!.ToString(), authToken);
        
        // handle connection closed event
        _connection.Closed += async error =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await _connection.StartAsync();
        };

        // handle message received event
        var tcs = new TaskCompletionSource<string>();
        _connection.On<string, string>("MessageHandler", (sender, message) => tcs.SetResult(message));

        // send location update to server
        await _connection.SendAsync("UpdateLocation", location);
        string message = await tcs.Task;
        var userLocation = await TestUserUtils.GetUserLocation(_client, userId);

        // assert
        Assert.NotNull(message);
        Assert.Equal($"User {userId} has moved to ({location.X_coord}, {location.Y_coord})", message);

        Assert.NotNull(userLocation);
        Assert.Equal(location.X_coord, userLocation!.X_coord);
        Assert.Equal(location.Y_coord, userLocation!.Y_coord);
    }
}