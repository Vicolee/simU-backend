using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class RemoveAgentHandler : IRequestHandler<RemoveAgentCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    public RemoveAgentHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(RemoveAgentCommand request, CancellationToken cancellationToken)
    {
        var creatorId = await _userRepository.GetUserIdFromIdentityId(request.CreatorIdentityId);
        await _worldRepository.RemoveAgent(request.Id, creatorId, request.AgentId);
        return Unit.Value;
    }
}