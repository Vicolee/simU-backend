using MediatR;
using SimU_GameService.Domain.Models; // Add this line to import the correct namespace

namespace SimU_GameService.Application.Services.Agents.Commands
{
    public record CreateAgentCommand(string Username, Guid CreatedByUser, int CollabDurationHours, string Description) : IRequest<Guid>;
}
