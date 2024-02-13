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
        // TO DO: CALL THE LLM SERVICE METHOD LEKINA WROTE IT WILL SEND A MESSAGE TO THE LLM SERVICE TO LET THEM KNOW
        // THAT THE RESPONSES HAVE BEEN POSTED AND THEY CAN SUMMARIZE THE USER'S CHARACTER
        return Unit.Value;
    }
}