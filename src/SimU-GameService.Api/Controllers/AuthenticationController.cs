using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

// TODO: adopt the CQRS pattern using MediatR to send commands and queries to the Application layer

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register", Name = "RegisterUser")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterUser([FromBody] RegisterRequest request)
    {
        // handle agent registration
        bool isAgent = request.IsAgent;
        if (isAgent)
        {
            Guid agentId = await _authenticationService.RegisterAgent(request.FirstName,
                request.LastName,
                request.IsAgent,
                request.Description ?? string.Empty);

            if (agentId == Guid.Empty)
            {
                // TODO: return a more appropriate error code
                return NotFound(new AuthenticationResponse(agentId, "Failed to register agent."));
            }
            return Ok(new AuthenticationResponse(agentId, "Agent registered."));
        }

        // handle user registration
        Guid userId = await _authenticationService.RegisterUser(
            request.FirstName,
            request.LastName,
            request.Email ?? string.Empty,
            request.Password ?? string.Empty);

        if (userId == Guid.Empty)
        {
            // TODO: return a more appropriate error code
            return NotFound(new AuthenticationResponse(userId, "User already registered."));
        }
        return Ok(new AuthenticationResponse(userId, "User registered."));
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
        // TODO: implement
        throw new NotImplementedException();
    }
}
