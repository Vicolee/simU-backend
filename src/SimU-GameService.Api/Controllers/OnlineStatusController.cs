using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Abstractions.Services;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OnlineStatusController : ControllerBase
{
    private readonly IOnlineStatusService _onlineStatusService;

    public OnlineStatusController(IOnlineStatusService onlineStatusService)
    {
        _onlineStatusService = onlineStatusService;
    }

    [HttpPost("{userId}", Name = "OnlineStatus")]
    public IActionResult OnlineStatus(Guid userId)
    {
        _onlineStatusService.SetOnlineStatus(userId);
        return Ok();
    }
}