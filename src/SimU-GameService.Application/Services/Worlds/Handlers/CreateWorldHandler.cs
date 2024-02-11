using MediatR;
using SimU_GameService.Application.Common.Abstractions;
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
        string? joinCode = await GenerateJoinCode();
        var world = new World(request.Name, request.Description, request.CreatorId, joinCode);
        await _worldRepository.CreateWorld(world);
        return world.Id;
    }

    // wrote the method below with the help of GitHub Co-Pilot.
    private async Task<string> GenerateJoinCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string joinCode;

        do
        {
            joinCode = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

        } while (await _worldRepository.JoinCodeExists(joinCode));

        return joinCode;
    }
}