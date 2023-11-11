using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
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
        // a user has already been created with that email
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
            // sets isAgent to false and description to empty string
            var user = new User(
                identityId,
                firstName,
                lastName,
                email,
                false,
                ""
            );

            await _userRepository.AddUser(user);
            return user.Id;

        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {e .InnerException.Message}");
            }
            throw new Exception("Failed to register user: " + e.Message);
        }
    }


public async Task<Guid> RegisterAgent(string firstName, string lastName, Boolean isAgent, string description)
{

    try
    {
        // sets identityID to empty string and email to empty string
        var user = new User(
            "",
            firstName,
            lastName,
            "",
            isAgent,
            description
        );

        await _userRepository.AddUser(user);
        return user.Id;

    }
    catch (Exception e)
    {
        if (e.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {e .InnerException.Message}");
        }
        throw new Exception("Failed to register agent: " + e.Message);
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
            // authToken for later JWT authentication
            // var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();

            // Extracting the status code from the response
            var statusCode = response.StatusCode;

            // Checking if the response status code is 200 OK
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                // Creating a variable called 'answer' and setting it equal to the status code
                var answer = (int)statusCode;

                // Now 'answer' contains the HTTP status code (200) from the response
                Console.WriteLine($"Status Code: {answer}");

                // Retrieve user from your repository using identityId
                var user = await _userRepository.GetUserByEmail(email) ?? throw new Exception("User not found.");
                return user.Id;
            }
            else
            {
                // Handle other status codes if necessary
                Console.WriteLine($"Unexpected Status Code: {statusCode}");
                return Guid.Empty;
            }
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }
}