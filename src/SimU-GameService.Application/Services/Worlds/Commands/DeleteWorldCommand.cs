using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record DeleteWorldCommand(Guid WorldId, Guid OwnerId) : IRequest<Unit>;