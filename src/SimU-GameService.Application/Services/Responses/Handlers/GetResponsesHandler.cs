using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.QuestionResponses.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class GetResponsesHandler : IRequestHandler<GetResponsesQuery, IEnumerable<Response>>
{
    private readonly IResponseRepository _responseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;

    public GetResponsesHandler(IResponseRepository responseRepository, IAgentRepository agentRepository, IUserRepository userRepository)
    {
        _responseRepository = responseRepository;
        _agentRepository = agentRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Response>> Handle(GetResponsesQuery request, CancellationToken cancellationToken)
    {
        var isUser = await _userRepository.GetUser(request.TargetId) != null;
        if (!isUser)
        {
            _ = await _agentRepository.GetAgent(request.TargetId) ??
                throw new NotFoundException(nameof(Character), request.TargetId);
        }
        return await _responseRepository.GetResponses(isUser, request.TargetId);
    }
}