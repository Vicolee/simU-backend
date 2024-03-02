using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Responses.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Responses.Handlers;

public class CheckIfUserQueryHandler : IRequestHandler<CheckIfUserQuery, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;

    public CheckIfUserQueryHandler(IUserRepository userRepository, IAgentRepository agentRepository)
    {
        _userRepository = userRepository;
        _agentRepository = agentRepository;
    }

    public async Task<bool> Handle(CheckIfUserQuery request, CancellationToken cancellationToken)
    {
        var isUser = await _userRepository.GetUser(request.TargetId) != null;
        if (!isUser)
        {
            _ = await _agentRepository.GetAgent(request.TargetId) ??
                throw new NotFoundException(nameof(Character), request.TargetId);
        }
        return isUser;
    }
}