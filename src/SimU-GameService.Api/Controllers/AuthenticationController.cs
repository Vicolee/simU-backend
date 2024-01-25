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
    public async Task<ActionResult<RegisterResponse>> RegisterUser([FromBody] RegisterRequest request)
    {
        // handle agent registration
        bool isAgent = request.IsAgent;
        if (isAgent)
        {
            Guid agentId = await _mediator.Send(new RegisterAgentCommand(
                request.FirstName,
                request.LastName,
                request.Description
                ?? throw new BadRequestException("Agent description is required.")));

            return Ok(new RegisterResponse(agentId, "Agent registered."));
        }

        // handle user registration
        Guid userId = await _mediator.Send(new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.Email ?? throw new BadRequestException("Email is required."),
            request.Password ?? throw new BadRequestException("Password is required.")))
            ?? throw new BadRequestException("User with given email already exists.");

        return Ok(new RegisterResponse(userId, "User registered."));
    }

    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<RegisterResponse>> LoginUser(LoginRequest request)
    {
        var authToken = await _mediator.Send(
            new LoginUserCommand(request.Email, request.Password))
            ?? throw new BadRequestException("Invalid email or password.");

        return Ok(new LoginResponse(authToken));
    }
}
