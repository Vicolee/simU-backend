using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Responses.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class PostResponseHandler : IRequestHandler<PostResponseCommand, Unit>
{
    private readonly IResponseRepository _responseRepository;

    public PostResponseHandler(IResponseRepository responseRepository) => _responseRepository = responseRepository;

    public async Task<Unit> Handle(PostResponseCommand request, CancellationToken cancellationToken)
    {
        var response = new Response(request.ResponderId, request.TargetCharacterId, request.QuestionId, request.Response);
        await _responseRepository.PostResponse(response);
        return Unit.Value;
    }
}