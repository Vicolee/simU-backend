using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record CreateWorldCommand(string Name, string Description, Guid CreatorId) : IRequest<Guid>;