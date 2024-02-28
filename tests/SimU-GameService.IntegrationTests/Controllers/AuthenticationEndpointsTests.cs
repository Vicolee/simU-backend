using System.Net;
using System.Net.Http.Json;
using SimU_GameService.IntegrationTests.TestUtils;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.IntegrationTests.Controllers;

public class AuthenticationEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;
    
    public AuthenticationEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _factory = factory;
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async void RegisterUser_WhenRequestValid_ShouldReturnOk()
    {
        // arrange
        var request = new RegisterRequest(
            Constants.User.TestUsername,
            Constants.User.TestEmail,
            Constants.User.TestPassword);

        // act
        HttpResponseMessage result = await _client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.RegisterUser, request);

        // handle case where test user already exists in database
        if (result.StatusCode != HttpStatusCode.OK)
        {
            var response = await result.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("email already exists", response);
        }
        else
        {
            var response = await result.Content.ReadFromJsonAsync<AuthenticationResponse>();

            // assert
            Assert.NotNull(response);

            Assert.IsType<Guid>(response.Id);
            Assert.IsType<string>(response.AuthToken);
        }
    }

    [Fact]
    public async void RegisterUser_WhenUserWithEmailExists_ShouldReturnBadRequest()
    {
        // arrange
        var request = new RegisterRequest(
            Constants.User.TestUsername,
            Constants.User.TestEmail,
            Constants.User.TestPassword);

        // act
        _ = await _client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.RegisterUser, request);
        var result = await _client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.RegisterUser, request);

        var response = await result.Content.ReadAsStringAsync();

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("email already exists", response);
    }

    [Fact]
    public async void LoginUser_WhenRequestValid_ShouldReturnOk()
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

        _ = await _client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.RegisterUser, registerRequest);
        
        // act

        // send login request
        var request = new LoginRequest(email, password);
        var result = await _client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.LoginUser, request);
        var response = await result.Content.ReadFromJsonAsync<AuthenticationResponse>();

        // assert
        Assert.NotNull(response);
        Assert.IsType<Guid>(response.Id);
        Assert.IsType<string>(response.AuthToken);
    }

    [Fact]
    public async void LoginUser_WhenUserDoesNotExist_ShouldReturnBadRequest()
    {
        // arrange
        var request = new LoginRequest(
            Constants.User.TestEmail,
            Constants.User.TestPassword);

        // act
        var result = await _client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.LoginUser, request);
        var response = await result.Content.ReadAsStringAsync();

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Invalid login credentials", response);
    }

    [Fact]
    public async void LogoutUser_WhenRequestValid_ShouldReturnNoContent()
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

        // send logout request
        var result = await _client.PostAsync(
            $"{Constants.Routes.AuthenticationEndpoints.LogoutUserPrefix}/{registerResponse!.Id}", null);

        // assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
    }
}