using MediatR;

namespace SimU_GameService.Application.Services.Agents.Commands;

public record CreateAgentCommand(
    string Username, string Description, Guid CreatorId, float CollabDurationHours) : IRequest<Guid>;
