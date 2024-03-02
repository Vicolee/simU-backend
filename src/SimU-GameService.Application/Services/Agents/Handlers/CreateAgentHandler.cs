using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Agents.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class CreateAgentHandler : IRequestHandler<CreateAgentCommand, Guid>
{
    private readonly IAgentRepository _agentRepository;
    private readonly IUserRepository _userRepository;

    public CreateAgentHandler(IAgentRepository agentRepository, IUserRepository userRepository)
    {
        _agentRepository = agentRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = new Agent(
            request.Username,
            request.CreatorId,
            request.CollabDurationHours,
            request.Description,
            request.SpriteURL,
            request.SpriteHeadshotURL
        );
        // grabs the creator of the agent's location and sets the newly created agent's location to theirs.
        var user = await _userRepository.GetUser(request.CreatorId) ?? throw new NotFoundException(nameof(User), request.CreatorId);
        Location creatorLocation = user.Location ?? new Location(0, 0);
        agent.UpdateLocation(creatorLocation.X_coord, creatorLocation.Y_coord);
        await _agentRepository.AddAgent(agent);

        return agent.Id;
    }
}