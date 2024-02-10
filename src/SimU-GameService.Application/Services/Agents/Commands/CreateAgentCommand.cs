using MediatR;

namespace SimU_GameService.Application.Services.Agents.Commands;

public record CreateAgentCommand(string Username,
    Guid Creator,
    int CollabDurationHours,
    string Description) : IRequest<Guid>;
