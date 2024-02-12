using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class AddUserHandler : IRequestHandler<AddUserCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    public AddUserHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(AddUserCommand request,
    CancellationToken cancellationToken)
    {
        await _userRepository.AddUserToWorld(request.UserId, request.WorldId);
        await _worldRepository.AddUser(request.WorldId, request.UserId);
        return Unit.Value; 
    }
}