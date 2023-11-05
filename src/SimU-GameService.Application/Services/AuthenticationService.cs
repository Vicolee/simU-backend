using System.Net.Http.Json;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;
using FirebaseAdmin.Auth;
using SimU_GameService.Application.Common.Authentication;

namespace SimU_GameService.Application.Services;

/// <summary>
/// Handles authentication logic in the Application layer.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly HttpClient _httpClient;

    public AuthenticationService(IUserRepository userRepository, HttpClient httpClient)
    {
        _userRepository = userRepository;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Registers a new user in the <see cref="IUserRepository"/>.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns> The user's ID. </returns>
    public async Task<Guid> RegisterUser(string firstName, string lastName, string email, string password)
    {

        if (await _userRepository.GetUserByEmail(email) != null)
        {
            return Guid.Empty;
        }

        var userArgs = new UserRecordArgs
        {
            Email = email,
            Password = password
        };

        try
        {
            var userRecord = FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);
            string identityId = userRecord.Result.Uid;
            
            var user = new User(
                identityId,
                firstName,
                lastName,
                email
            );

            await _userRepository.AddUser(user);
            return user.Id;

        }
        catch (Exception e)
        {
            throw new Exception("Failed to register user: " + e.Message);
        }
    }

    /// <summary>
    /// Logs in an existing user.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns> The user's ID. </returns>
    /// <exception cref="Exception"></exception>
    public async Task<Guid> LoginUser(string email, string password)
    {
        try
        {
            var request = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync("", request);
            var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();


            Console.Write(response);

            // Retrieve user from your repository using identityId
            var user = await _userRepository.GetUserByEmail(email) ?? throw new Exception("User not found.");
            return user.Id;
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }
}