using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class AddUserToWorldHandler : IRequestHandler<AddUserToWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public AddUserToWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(AddUserToWorldCommand request,
    CancellationToken cancellationToken) => await _worldRepository.AddUserToWorld(request.WorldId, request.UserId);
}