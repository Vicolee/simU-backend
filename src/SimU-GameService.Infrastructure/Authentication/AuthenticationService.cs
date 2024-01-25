using System.Net.Http.Json;
using FirebaseAdmin.Auth;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;

namespace SimU_GameService.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> LoginUser(string email, string password)
    {
        var request = new 
        {
            email,
            password,
            returnSecureToken = true
        };

        var response = await _httpClient.PostAsJsonAsync("", request);
        var authToken = await response.Content.ReadFromJsonAsync<AuthToken>()
            ?? throw new BadRequestException("Invalid login credentials.");
        return authToken.IdToken;
    }

    public async Task<string> RegisterUser(
        string email, string password, CancellationToken cancellationToken)
    {
        var userArgs = new UserRecordArgs
        {
            Email = email,
            Password = password
        };

        var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(
            userArgs, cancellationToken);
        return userRecord.Uid;
    }
}