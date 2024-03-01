using System.Net;
using System.Net.Http.Json;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.IntegrationTests.TestUtils;

public static class TestUserUtils
{
    internal static async Task<AuthenticationResponse?> RegisterUser(
        HttpClient client,
        string email,
        string username = Constants.User.TestUsername,
        string password = Constants.User.TestPassword)
    {
        var request = new RegisterRequest(username, email, password);
        var result = await client.PostAsJsonAsync(
            Constants.Routes.AuthenticationEndpoints.RegisterUser, request);

        return result.StatusCode == HttpStatusCode.OK
            ? await result.Content.ReadFromJsonAsync<AuthenticationResponse>()
            : null;      
    }

    internal static async Task<Location?> GetUserLocation(HttpClient client, Guid userId)
    {
        var route = $"{Constants.Routes.UsersEndpoints.BaseUri}/{userId}";
        var user = await client.GetFromJsonAsync<UserResponse>(route);
        return user!.Location;
    }
}