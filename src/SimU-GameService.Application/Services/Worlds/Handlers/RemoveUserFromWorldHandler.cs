using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class RemoveUserFromWorldHandler : IRequestHandler<RemoveUserFromWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public RemoveUserFromWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(RemoveUserFromWorldCommand request, CancellationToken cancellationToken)
    {
        try {
            await _worldRepository.RemoveUserFromWorld(request.WorldId, request.UserId);
            return Unit.Value;
        }
        catch
        {
            throw;
        }
    }
}