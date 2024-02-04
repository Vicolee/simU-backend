using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record AddUserToWorldCommand(Guid WorldId, Guid UserId) : IRequest<Unit>;
