using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Application.Common;
using SimU_GameService.Application.Services;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IHubContext<UnityClientHub, IUnityClient> _hubContext;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(
        IHubContext<UnityClientHub, IUnityClient> hubContext,
        IUserRepository userRepository)
    {
        _hubContext = hubContext;
        _authenticationService = new AuthenticationService(userRepository);
    }

    [HttpPost("register", Name = "RegisterUser")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterUser([FromBody] RegisterRequest request)
    {
        // TODO: update to pass password for Firebase auth
        var userId = await _authenticationService.RegisterUser(
            request.FirstName,
            request.LastName,
            request.Email);

        if (userId == Guid.Empty)
        {
            return NotFound(new AuthenticationResponse(userId.ToString(), "User already registered."));
        }

        await _hubContext.Clients.All.ReceiveMessage("Server",
            $"New user {request.FirstName} with ID {userId} has registered.");

        return Ok(new AuthenticationResponse(userId.ToString(), "User registered."));
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<AuthenticationResponse>> LoginUser(LoginRequest request)
    {
        var userId = await _authenticationService.LoginUser(
            request.Email,
            request.Password);

        if (userId == Guid.Empty)
        {
            return NotFound(new AuthenticationResponse(userId.ToString(), "User not found."));
        }

        await _hubContext.Clients.All.ReceiveMessage("Server",
            $"User {request.Email} with ID {userId} has logged in.");

        return Ok(new AuthenticationResponse(userId.ToString(), "User logged in."));
    }

    [HttpPut("{userId}/logout", Name = "LogoutUser")]
    public Task<ActionResult> LogoutUser(Guid userId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }
}
