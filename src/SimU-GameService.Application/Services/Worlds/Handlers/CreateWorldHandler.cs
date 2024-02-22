using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class CreateWorldHandler : IRequestHandler<CreateWorldCommand, World>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;
    private readonly Random _random = new();

    public CreateWorldHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<World> Handle(CreateWorldCommand request, CancellationToken cancellationToken)
    {
        var world = new World(
            request.Name,
            request.Description,
            request.CreatorId,
            await GenerateWorldCode());

        await _worldRepository.CreateWorld(world);
        await _userRepository.AddUserToWorld(request.CreatorId, world.Id, true);
        return world;
    }

    private async Task<string> GenerateWorldCode()
    {
        string worldCode;
        const string validCharacters =  "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        do
        {
            worldCode = new(Enumerable.Repeat(validCharacters, 8)
                .Select(s => s[_random.Next(s.Length)])
                .ToArray());

        } while (await _worldRepository.WorldCodeExists(worldCode));
        
        return worldCode.ToLower();
    }
}