using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common;
using SimU_GameService.Application.Services.Agents.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class UpdateAgentSpriteHandler : IRequestHandler<UpdateAgentSpriteCommand, Unit>
{
    private readonly IAgentRepository _agentRepository;

    public UpdateAgentSpriteHandler(IAgentRepository agentRepository) => _agentRepository = agentRepository;

    public async Task<Unit> Handle(UpdateAgentSpriteCommand request,
        CancellationToken cancellationToken) => await _agentRepository.UpdateAgentSprite(request.AgentId, request.SpriteURL, request.SpriteHeadshotURL);
}