using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class CreateWorldHandler : IRequestHandler<CreateWorldCommand, Guid>
{
    private readonly IWorldRepository _worldRepository;
    private static readonly Random _random = new Random();

    public CreateWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Guid> Handle(CreateWorldCommand request, CancellationToken cancellationToken)
    {
        string? worldCode = await GenerateWorldCode();
        var world = new World(request.Name, request.Description, request.CreatorId, worldCode);
        await _worldRepository.CreateWorld(world);
        return world.Id;
    }

    // wrote the method below with the help of GitHub Co-Pilot.
    private async Task<string> GenerateWorldCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string worldCode;

        do
        {
            worldCode = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

        } while (await _worldRepository.WorldCodeExists(worldCode));

        return worldCode;
    }
}