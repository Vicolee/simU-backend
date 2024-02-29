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

    private static async Task<HubConnection> StartConnectionAsync(
        HttpMessageHandler handler, string hubURL, string authToken)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(hubURL, options =>
            {
                options.HttpMessageHandlerFactory = _ => handler;
                options.AccessTokenProvider = () => Task.FromResult(authToken ?? default);
            })
            .Build();

        await hubConnection.StartAsync();    
        return hubConnection;
    }
            

    [Fact]
    public async void UpdateLocation_WhenUserExists_ShouldWork()
    {
        // arrange        
        // register user to get ID and auth token
        var registerResponse = await TestUserUtils.RegisterUser(
            _client,
            Constants.User.TestEmail);

        var userId = registerResponse!.Id;
        var authToken = registerResponse!.AuthToken;

        // generate random location
        Random random = new();
        var location = new Location(random.Next(0, 100), random.Next(0, 100));

        // initialize SignalR connection
        var hubURL = new Uri($"{Constants.Routes.BaseUri}{Constants.Routes.UnityHub.BaseUri}");
        _connection = await StartConnectionAsync(_factory.Server.CreateHandler(), hubURL!.ToString(), authToken);
        
        // handle connection closed event
        _connection.Closed += async error =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await _connection.StartAsync();
        };

        // handle message received event
        var tcs = new TaskCompletionSource<(Guid, Location)>();
        _connection.On<Guid, Location>("UpdateLocationHandler", (sender, location) => tcs.SetResult((sender, location)));
        
        // act
        // send location update to server
        await _connection.SendAsync("UpdateLocation", location);
        var (user_id, user_location) = await tcs.Task;
        var userLocation = await TestUserUtils.GetUserLocation(_client, userId);

        // assert
        Assert.Equal(userId, user_id);
        Assert.NotNull(userLocation);
        Assert.Equal(location.X_coord, user_location!.X_coord);
        Assert.Equal(location.Y_coord, user_location!.Y_coord);

        Assert.NotNull(userLocation);
        Assert.Equal(location.X_coord, userLocation!.X_coord);
        Assert.Equal(location.Y_coord, userLocation!.Y_coord);
    }

    [Fact]
    public async Task SendChat_WhenSenderAndReceiverExist_ShouldWorkAsync()
    {
        // arrange

        // register sender and receiver
        var senderResponse = await TestUserUtils.RegisterUser(_client,
            Constants.User.TestUsername,
            Constants.User.TestEmail,
            Constants.User.TestPassword);

        var receiverResponse = await TestUserUtils.RegisterUser(_client,
            Constants.User.TestUsername,
            Constants.User.TestEmail,
            Constants.User.TestPassword);

        // initialize connection for sender and receiver        
        var hubURL = new Uri($"{Constants.Routes.BaseUri}{Constants.Routes.UnityHub.Route}");
        _connection = await StartConnectionAsync(
            _factory.Server.CreateHandler(), hubURL!.ToString(), senderResponse!.AuthToken);
        var receiverConnection = await StartConnectionAsync(
            _factory.Server.CreateHandler(), hubURL!.ToString(), receiverResponse!.AuthToken);

        // register handler for message received and closed connecting events
        _connection.Closed += async error =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await _connection.StartAsync();
        };

        receiverConnection.Closed += async error =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await receiverConnection.StartAsync();
        };
        
        var senderTcs = new TaskCompletionSource<(Guid, Location)>();
        _connection.On<Guid, Location>("UpdateLocationHandler", (sender, location) => senderTcs.SetResult((sender, location)));

        var receiverTcs = new TaskCompletionSource<(Guid, Location)>();
        receiverConnection.On<Guid, Location>("UpdateLocationHandler", (sender, location) => receiverTcs.SetResult((sender, location)));
        

        // act
        // send chat message from sender to receiver
        

        // assert
        // check if receiver received the message
    }
}