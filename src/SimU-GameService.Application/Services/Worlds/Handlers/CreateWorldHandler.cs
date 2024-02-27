using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Abstractions.Services;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class CreateWorldHandler : IRequestHandler<CreateWorldCommand, World>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    private readonly ILLMService _llmService;
    private readonly Random _random = new();

    public CreateWorldHandler(IWorldRepository worldRepository, IUserRepository userRepository, ILLMService llmService)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
        _llmService = llmService;
    }
    public async Task<World> Handle(CreateWorldCommand request, CancellationToken cancellationToken)
    {
        // checks to see if the user exists
        if (await _userRepository.GetUser(request.CreatorId) == null)
        {
            throw new NotFoundException(nameof(User), request.CreatorId);
        }
        var world = new World(
            request.Name,
            request.Description,
            request.CreatorId,
            await GenerateWorldCode());

        await _worldRepository.CreateWorld(world);
        await _userRepository.AddUserToWorld(request.CreatorId, world.Id, true);
        // updates the URL of the thumbnail
        string thumbnailURLString = await _llmService.GenerateWorldThumbnail(world.Id, world.CreatorId, world.Description);
        Uri thumbnailURL = new(thumbnailURLString);
        await _worldRepository.UpdateWorldThumbnail(world.Id, thumbnailURL);

        // Retrieve the updated world object from the database
        world = await _worldRepository.GetWorld(world.Id) ?? throw new NotFoundException(nameof(World), world.Id);
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