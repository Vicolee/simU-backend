using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class DeleteWorldHandler : IRequestHandler<DeleteWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public DeleteWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(DeleteWorldCommand request, CancellationToken cancellationToken)
    {
        try {
            await _worldRepository.DeleteWorld(request.WorldId, request.OwnerId);
            return Unit.Value;
        }
        catch
        {
            throw;
        }
    }
}