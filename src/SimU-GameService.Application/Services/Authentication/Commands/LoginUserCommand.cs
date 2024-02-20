using MediatR;

namespace SimU_GameService.Application.Services.Authentication.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<(Guid, string)>;