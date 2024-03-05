using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Api.DomainEvents.Events;
using SimU_GameService.Application.Services.Authentication.Commands;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register", Name = "RegisterUser")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterUser(RegisterRequest request)
    {
        await _mediator.Send(new RegisterUserCommand(request.Username, request.Email, request.Password));
        var (id, authToken) = await _mediator.Send(new LoginUserCommand(request.Email, request.Password));
        return Ok(new AuthenticationResponse(id, authToken));
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<AuthenticationResponse>> LoginUser(LoginRequest request)
    {
        var (id, authToken) = await _mediator.Send(
            new LoginUserCommand(request.Email, request.Password));
        return Ok(new AuthenticationResponse(id, authToken));
    }

    [HttpPost("logout/{id}", Name = "LogoutUser")]
    public async Task<ActionResult> LogoutUser(Guid id)
    {
        await _mediator.Send(new LogoutUserCommand(id));
        return NoContent();
    }
}
