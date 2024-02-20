using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class PostDescriptionHandler : IRequestHandler<PostDescriptionCommand, (Uri, Uri)>
{
    private readonly IAgentRepository _agentRepository;
    private readonly ILLMService _llmService;

    public PostDescriptionHandler(IAgentRepository agentRepository, ILLMService llmService)
    {
        _agentRepository = agentRepository;
        _llmService = llmService;
    }

    public async Task<(Uri, Uri)> Handle(PostDescriptionCommand request, CancellationToken cancellationToken)
    {
        await _agentRepository.UpdateDescription(request.AgentId, request.Description);
        var spriteURLs = await _llmService.GenerateSprites(request.AgentId, request.Description);
        return (spriteURLs["sprite_URL"],spriteURLs["sprite_headshot_URL"]);
    }
}