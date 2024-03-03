using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class DeleteWorldHandler : IRequestHandler<DeleteWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    public DeleteWorldHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteWorldCommand request, CancellationToken cancellationToken)
    {
        await _worldRepository.DeleteWorld(request.WorldId, request.OwnerId);
        return Unit.Value;
    }
}