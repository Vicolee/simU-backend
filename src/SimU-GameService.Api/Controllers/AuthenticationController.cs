using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register/user", Name = "RegisterUser")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterUser([FromBody] RegisterRequestUser request)
    {

        Guid userId = await _authenticationService.RegisterUser(
        request.Username,
        request.Email,
        request.Password);
        if (userId == Guid.Empty)
        {
            return NotFound(new AuthenticationResponse(userId, "User already registered."));
        } else {
            return Ok(new AuthenticationResponse(userId, "User registered."));
        }
        }
    }

    [HttpPost("register/agent", Name = "RegisterAgent")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAgent([FromBody] RegisterRequestAgent request)
    {
        Boolean isAgent = request.IsAgent;

        // Creates an LLM, so email and password to set to empty string
        if (isAgent) {
            Guid userId = await _authenticationService.RegisterAgent(
            request.Username,
            request.CreatedByUser,
            request.CollabDurationHours,
            request.IsAgent,
            request.Description);
            if (userId == Guid.Empty)
            {
                return NotFound(new AuthenticationResponse(userId, "Failed to register agent."));
            } else {
                return Ok(new AuthenticationResponse(userId, "Agent registered."));
            }
        } else {
            return NotFound(new AuthenticationResponse(Guid.Empty, "User is not an agent."));
        }
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<AuthenticationResponse>> LoginUser(LoginRequest request)
    {
        var userId = await _authenticationService.LoginUser(
            request.Email,
            request.Password);

        if (userId == Guid.Empty)
        {
            return NotFound(new AuthenticationResponse(userId, "User not found."));
        }

        return Ok(new AuthenticationResponse(userId, "User logged in."));
    }

    [HttpPut("{userId}/logout", Name = "LogoutUser")]
    public Task<ActionResult> LogoutUser(Guid userId)
    {
        throw new NotImplementedException();
    }
}
