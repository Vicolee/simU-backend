using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class RemoveUserFromWorldHandler : IRequestHandler<RemoveUserCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public RemoveUserFromWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        await _worldRepository.RemoveUser(request.WorldId, request.UserId);
        return Unit.Value;
    }
}