using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Common;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Agents.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class PostVisualDescriptionHandler : IRequestHandler<PostVisualDescriptionCommand, (Uri, Uri)>
{
    private readonly IAgentRepository _agentRepository;
    private readonly ILLMService _agentService;

    public PostVisualDescriptionHandler(IAgentRepository agentRepository, ILLMService agentService)
    {
        _agentRepository = agentRepository;
        _agentService = agentService;
    }

    public async Task<(Uri, Uri)> Handle(PostVisualDescriptionCommand request,
        CancellationToken cancellationToken)
    {
        Dictionary<string, string>? spriteUrls = await _agentService.GenerateAgentSprite(request.AgentId, request.Description) ?? throw new BadRequestException("Failed to generate agent sprite URLs on LLM Service.");
        Uri avatarURL = new(spriteUrls["avatarURL"]);
        Uri headshotURL = new(spriteUrls["headshotURL"]);

        await _agentRepository.UpdateSprite(request.AgentId, avatarURL, headshotURL);
        return (avatarURL, headshotURL);
    }
}