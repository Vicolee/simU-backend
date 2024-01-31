using MediatR;

namespace SimU_GameService.Application.Services.Authentication.Commands;

public record RegisterUserCommand(
    string Username, string Email, string Password) : IRequest<Unit>;