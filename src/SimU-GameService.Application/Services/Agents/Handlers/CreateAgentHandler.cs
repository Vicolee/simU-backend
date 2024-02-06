using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Agents.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class CreateAgentHandler : IRequestHandler<CreateAgentCommand, Guid>
{
    private readonly IAgentRepository _agentRepository;

    public CreateAgentHandler(IAgentRepository agentRepository) => _agentRepository = agentRepository;

    public async Task<Guid> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = new Agent(
            request.Username,
            request.CreatedByUser,
            request.CollabDurationHours,
            request.Description
        );
        await _agentRepository.AddAgent(agent);
        return agent.Id;
    }
}