using System.Net;
using System.Net.Http.Json;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.IntegrationTests.TestUtils;

namespace SimU_GameService.IntegrationTests.Controllers;

public class UsersEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;

    public UsersEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async void GetUser_WhenUserExists_ShouldReturnOk()
    {
        // arrange
        // send register request to create user
        var username = Constants.User.TestUsername;
        var email = Constants.User.TestEmail;
        var password = Constants.User.TestPassword;

        var registerRequest = new RegisterRequest(
            username,
            email,
            password);

        var registerResult = await _client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.RegisterUser, registerRequest);
        var registerResponse = await registerResult.Content.ReadFromJsonAsync<AuthenticationResponse>();

        // act
        var result = await _client.GetAsync(
            $"{Constants.Routes.UsersEndpoints.BaseUri}/{registerResponse!.Id}");
        var response = await result.Content.ReadFromJsonAsync<UserResponse>();

        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.Equal(registerResponse.Id, response.Id);
        Assert.Equal(username, response.Username);
        Assert.Equal(email, response.Email);
    }
}