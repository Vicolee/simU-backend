using System.Net;
using System.Net.Http.Json;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.IntegrationTests.TestUtils;

namespace SimU_GameService.IntegrationTests.Controllers;

public class WorldsEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;

    public WorldsEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async void CreateWorld_WhenWorldDoesNotExist_ShouldReturnOk()
    {
        // arrange
        // register user to create world
        var creator = await TestUserUtils.RegisterUser(
            _client, Constants.User.TestEmail);
        
        // act
        var request = new CreateWorldRequest(
            creator!.Id,
            Constants.World.TestWorldName,
            Constants.World.TestWorldDescription);

        var result = await _client.PostAsJsonAsync(Constants.Routes.WorldsEndpoints.BaseUri, request);
        var response = await result.Content.ReadFromJsonAsync<WorldResponse>();

        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Description, response.Description);
        Assert.Equal(request.CreatorId, response.CreatorId);
    }

    [Fact]
    public async void AddUserToWorld_WhenUserExists_ShouldReturnNoContent()
    {
        // arrange
        // create world
        var creator = await TestUserUtils.RegisterUser(_client, Constants.User.TestEmail);
        var world = await TestWorldUtils.CreateWorld(_client, creator!.Id); 
        
        // register user to add to world
        var user = await TestUserUtils.RegisterUser(_client, Constants.User.TestEmail);     
        
        // act
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{world!.Id}/users/{user!.Id}";
        var result = await _client.PostAsync(route, null);        
        var worldUsers = await TestWorldUtils.GetWorldUsers(_client, world!.Id);
        
        // assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.NotNull(worldUsers);
        Assert.Contains(worldUsers, u => u!.Id == user!.Id);
    }

    [Fact]
    public async void AddAgentToWorld_WhenAgentExists_ShouldReturnNoContentAsync()
    {
        // arrange
        // create world
        var creator = await TestUserUtils.RegisterUser(_client, Constants.User.TestEmail);
        var world = await TestWorldUtils.CreateWorld(_client, creator!.Id);
        
        // register agent to add to world
        var agent = await TestAgentUtils.CreateAgent(_client, creator.Id);
        
        // act
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{world!.Id}/agents/{agent!.Id}";
        var result = await _client.PostAsync(route, null);
        var worldAgents = await TestWorldUtils.GetWorldAgents(_client, world!.Id);
        
        // assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.NotNull(worldAgents);
        Assert.Contains(worldAgents, a => a!.Id == agent!.Id);
    }

    [Fact]
    public async void GetWorld_WhenWorldExists_ShouldReturnOk()
    {
        // arrange
        // register user to create world
        var creator = await TestUserUtils.RegisterUser(
            _client, Constants.User.TestEmail);
        
        // create world
        var world = await TestWorldUtils.CreateWorld(
            _client, creator!.Id);
        
        // act
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{world!.Id}";
        var result = await _client.GetAsync(route);
        var response = await result.Content.ReadFromJsonAsync<WorldResponse>();


        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.Equal(world.Id, response.Id);
        Assert.Equal(world.CreatorId, response.CreatorId);
        Assert.Equal(world.Name, response.Name);
        Assert.Equal(world.Description, response.Description);
        Assert.Equal(world.WorldCode, response.WorldCode);
        Assert.Equal(world.Thumbnail_URL, response.Thumbnail_URL);
    }

    [Fact]
    public async void GetWorldIdFromWorldCode_WhenWorldExists_ShouldReturnOk()
    {
        // arrange
        // register user to create world
        var creator = await TestUserUtils.RegisterUser(
            _client, Constants.User.TestEmail);
        
        // create world
        var world = await TestWorldUtils.CreateWorld(
            _client, creator!.Id);
        
        // act
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{world!.WorldCode}";
        var result = await _client.GetAsync(route);
        var response = await result.Content.ReadFromJsonAsync<IdResponse>();

        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.Equal(world.Id, response.Id);
    }

    [Fact]
    public async void GetWorldUsers_WhenWorldExists_ShouldReturnOk()
    {
        // arrange
        // register user to create world
        var creator = await TestUserUtils.RegisterUser(
            _client, Constants.User.TestEmail);
        
        // create world
        var world = await TestWorldUtils.CreateWorld(
            _client, creator!.Id);
        
        // act
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{world!.Id}/users";
        var result = await _client.GetAsync(route);
        var response = await result.Content.ReadFromJsonAsync<IEnumerable<WorldUserResponse>>();

        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.Contains(response, u => u!.Id == creator.Id);
    }

    [Fact]
    public async Task DeleteWorld_WhenWorldExists_ShouldReturnNoContentAsync()
    {
        // arrange
        // register user to create world
        var creator = await TestUserUtils.RegisterUser(
            _client, Constants.User.TestEmail);
        
        // create world
        var world = await TestWorldUtils.CreateWorld(
            _client, creator!.Id);
        
        // act
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{world!.Id}";
        var result = await _client.DeleteAsync(route);
        var worldResponse = await TestWorldUtils.GetWorld(_client, world!.Id);

        // assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.Null(worldResponse);
    }
}