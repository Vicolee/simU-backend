using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Authentication.Commands;
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
        var idTokenPair = await _mediator.Send(new LoginUserCommand(request.Email, request.Password));
        return Ok(new AuthenticationResponse(idTokenPair.Item1, idTokenPair.Item2));
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<AuthenticationResponse>> LoginUser(LoginRequest request)
    {
        var idTokenPair = await _mediator.Send(
            new LoginUserCommand(request.Email, request.Password))
            ?? throw new BadRequestException("Invalid email or password.");
        return Ok(new AuthenticationResponse(idTokenPair.Item1, idTokenPair.Item2));
    }
}
