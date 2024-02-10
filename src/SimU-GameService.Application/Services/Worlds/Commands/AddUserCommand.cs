using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record AddUserCommand(Guid WorldId, Guid UserId) : IRequest<Unit>;
