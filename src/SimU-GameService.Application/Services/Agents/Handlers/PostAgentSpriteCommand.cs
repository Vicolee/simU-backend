using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Common;
using SimU_GameService.Application.Services.Agents.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class PostAgentSpriteHandler : IRequestHandler<PostAgentSpriteCommand, Dictionary<string, string>?>
{
    private readonly IAgentRepository _agentRepository;
    private readonly ILLMService _agentService;

    public PostAgentSpriteHandler(IAgentRepository agentRepository, ILLMService agentService)
    {
        _agentRepository = agentRepository;
        _agentService = agentService;
    }

    public async Task<Dictionary<string, string>?> Handle(PostAgentSpriteCommand request,
        CancellationToken cancellationToken)
    {
        Dictionary<string, string>? spriteUrls = await _agentService.GenerateAgentSprite(request.AgentId, request.Description);
        if (spriteUrls == null)
        {
            return null;
        }
        Uri avatarURL = new(spriteUrls["avatarURL"]);
        Uri headshotURL = new(spriteUrls["headshotURL"]);

        await _agentRepository.UpdateAgentSprite(request.AgentId, avatarURL, headshotURL);
        return spriteUrls;
    }
}