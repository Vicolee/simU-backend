using MediatR;

namespace SimU_GameService.Application.Services.Authentication.Commands;

public record RegisterUserCommand(
    string FirstName, string LastName, string Email, string Password) : IRequest<Unit>;