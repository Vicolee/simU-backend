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

    [HttpPost("register", Name = "RegisterUser")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterUser([FromBody] RegisterRequest request)
    {
        Guid userId = await _authenticationService.RegisterUser(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        if (userId == Guid.Empty)
        {
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
