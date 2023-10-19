using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [HttpPost("register", Name = "RegisterUser")]
    public Task<RegisterUserResponse> RegisterUser([FromBody] RegisterUserRequest request)
    {
        var response = new RegisterUserResponse(request.Username, request.Email);
        return Task.FromResult(response);
    }
}