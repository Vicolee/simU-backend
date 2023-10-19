using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IHubContext<UnityClientHub, IUnityClient> _hubContext;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IHubContext<UnityClientHub, IUnityClient> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    [HttpPost("register", Name = "RegisterUser")]
    public async Task<RegisterUserResponse> RegisterUser([FromBody] RegisterUserRequest request)
    {
        var response = new RegisterUserResponse(request.Username, request.Email);
        await _hubContext.Clients.All.ReceiveMessage("Server", $"{response}");
        return response;
    }
}
