using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Authentication.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Authentication.Handlers;

public class RegisterAgentHandler : IRequestHandler<RegisterAgentCommand, Guid>
{
    private readonly IAgentRepository _agentRepository;

    public RegisterAgentHandler(IAgentRepository agentRepository) => _agentRepository = agentRepository;

    public async Task<Guid> Handle(RegisterAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = new Agent(
            request.Username,
            Guid.Empty,
            0,
            request.Description ?? string.Empty
        );

        await _agentRepository.AddAgent(agent);
        return agent.Id;         
    }
}