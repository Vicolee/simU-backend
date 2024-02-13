using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;
using SimU_GameService.Application.Common.Exceptions;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class RemoveUserFromWorldHandler : IRequestHandler<RemoveUserFromWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public RemoveUserFromWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(RemoveUserFromWorldCommand request, CancellationToken cancellationToken)
    {
        try {
            await _worldRepository.RemoveUser(request.WorldId, request.UserId, request.OwnerId);
            return Unit.Value;
        }
        catch
        {
            throw;
        }
    }
}