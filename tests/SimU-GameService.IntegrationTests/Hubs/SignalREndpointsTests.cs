using Microsoft.AspNetCore.SignalR.Client;
using SimU_GameService.Contracts.Responses;
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
        HttpMessageHandler handler,
        string authToken,
        string hubURL = $"{Constants.Routes.BaseUri}{Constants.Routes.UnityHub.BaseUri}")
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(hubURL, options =>
            {
                options.HttpMessageHandlerFactory = _ => handler;
                options.AccessTokenProvider = () => Task.FromResult(authToken ?? default);
            })
            .Build();

        await hubConnection.StartAsync();
        hubConnection.Closed += async error =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        };
        return hubConnection;
    }
            

    [Fact]
    public async Task UpdateLocation_WhenUserExists_ShouldWorkAsync()
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
        _connection = await StartConnectionAsync(_factory.Server.CreateHandler(), authToken);

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
    public async Task SendChat_WhenSenderAndReceiverAreUsers_ShouldWorkAsync()
    {
        // arrange
        // register sender and receiver
        var sender = await TestUserUtils.RegisterUser(_client, Constants.User.TestEmail);
        var receiver = await TestUserUtils.RegisterUser(_client, $"TestReceiver{DateTime.Now.Ticks}@SimU.com");

        // initialize connection for sender and receiver
        _connection = await StartConnectionAsync(_factory.Server.CreateHandler(), sender!.AuthToken);
        var receiverConnection = await StartConnectionAsync(_factory.Server.CreateHandler(), receiver!.AuthToken);

        // register handlers for message received event        
        var senderTcs = new TaskCompletionSource<ChatResponse>();
        var receiverTcs = new TaskCompletionSource<ChatResponse>();

        _connection.On<ChatResponse>("ChatHandler", senderTcs.SetResult);
        receiverConnection.On<ChatResponse>("ChatHandler", receiverTcs.SetResult);
        
        // act
        // send chat message from sender to receiver
        var message = "This is a test message.";
        await _connection.SendAsync("SendChat", receiver!.Id, message);
        
        var senderChatResponse = await senderTcs.Task;
        var receiverChatResponse = await receiverTcs.Task;

        // assert
        Assert.NotNull(senderChatResponse);
        Assert.Equal(sender.Id, senderChatResponse.SenderId);
        Assert.Equal(receiver.Id, senderChatResponse.ReceiverId);
        Assert.Equal(message, senderChatResponse.Content);

        Assert.NotNull(receiverChatResponse);
        Assert.Equal(sender.Id, receiverChatResponse.SenderId);
        Assert.Equal(receiver.Id, receiverChatResponse.ReceiverId);
        Assert.Equal(message, receiverChatResponse.Content);
    }

    [Fact]
    public async Task SendChat_WhenReceiverIsAgent_ShouldWorkAsync()
    {
        // arrange
        // register sender as user and receiver as an agent
        var sender = await TestUserUtils.RegisterUser(_client, Constants.User.TestEmail);
        var receiver = await TestAgentUtils.CreateAgent(_client, sender!.Id);

        // initialize connection for sender
        _connection = await StartConnectionAsync(_factory.Server.CreateHandler(), sender!.AuthToken);

        // register handler for message received event
        var sentChatTcs = new TaskCompletionSource<ChatResponse>();
        var agentResponseTcs = new TaskCompletionSource<ChatResponse>();

        _connection.On<ChatResponse>("ChatHandler", response =>
        {
            if (sentChatTcs.Task.IsCompleted)
            {
                agentResponseTcs.SetResult(response);
            }
            else
            {
                sentChatTcs.SetResult(response);
            }
        });
        
        // act
        // send chat message from sender to receiver
        var message = "This is a test message.";
        await _connection.SendAsync("SendChat", receiver!.Id, message);
        
        var sentChatResponse = await sentChatTcs.Task;
        var receivedChatResponse = await agentResponseTcs.Task;

        // assert
        Assert.NotNull(sentChatResponse);
        Assert.Equal(sender.Id, sentChatResponse.SenderId);
        Assert.Equal(receiver.Id, sentChatResponse.ReceiverId);
        Assert.Equal(message, sentChatResponse.Content);

        Assert.NotNull(receivedChatResponse);
        Assert.Equal(receiver.Id, receivedChatResponse.SenderId);
        Assert.Equal(sender.Id, receivedChatResponse.ReceiverId);
    }
}