using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class AddUserHandler : IRequestHandler<AddUserCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public AddUserHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(AddUserCommand request,
    CancellationToken cancellationToken) => await _worldRepository.AddUser(request.WorldId, request.UserId);
}