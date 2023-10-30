using SimU_GameService.Application.Common;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services;

/// <summary>
/// Handles authentication logic in the Application layer.
/// </summary>
public class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    
    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Registers a new user in the <see cref="IUserRepository"/>.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns> The user's ID. </returns>
    public async Task<Guid> RegisterUser(string firstName, string lastName, string email)
    {
        // TODO: use Firebase authentication to get identity ID
        // For now, use Guid.NewGuid()

        var identityId = Guid.NewGuid();
        var user = new User(
            identityId,
            firstName,
            lastName,
            email);

        if (await _userRepository.GetUserByEmail(email) != null)
        {
            return Guid.Empty;
        }

        await _userRepository.AddUser(user);
        return user.Id;
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
            // TODO: use Firebase authentication to log in user. For now, every user is logged in.
            var user = await _userRepository.GetUserByEmail(email) ?? throw new Exception("User not found.");
            return user.Id;
        }
        catch (Exception)
        {
            return Guid.Empty;
        }
    }
}
