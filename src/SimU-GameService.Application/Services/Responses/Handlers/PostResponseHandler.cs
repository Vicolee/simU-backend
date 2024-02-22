using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Responses.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class PostResponseHandler : IRequestHandler<PostResponseCommand, Unit>
{
    private readonly IResponseRepository _responseRepository;
    private readonly ILLMService _agentService;

    public PostResponseHandler(IResponseRepository responseRepository, ILLMService agentService) {
        _responseRepository = responseRepository;
        _agentService = agentService;
    }

    public async Task<Unit> Handle(PostResponseCommand request, CancellationToken cancellationToken)
    {
        var response = new Response(request.ResponderId, request.TargetCharacterId, request.QuestionId, request.Response);
        await _responseRepository.PostResponse(response);
        // TO DO: TALK TO LEKINA ABOUT THE POSTRESPONSE vs POSTRESPONSES
        return Unit.Value;
    }
}