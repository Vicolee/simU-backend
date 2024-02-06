using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.QuestionResponses.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class PostResponseHandler : IRequestHandler<PostResponseCommand, Unit>
{
    private readonly IQuestionResponseRepository _questionResponseRepository;

    public PostResponseHandler(IQuestionResponseRepository questionResponseRepository)
    {
        _questionResponseRepository = questionResponseRepository;
    }

    /// <summary>
    /// Returns the AI generated summary for an agent based off the
    /// information users provided about it during its incubation process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Unit> Handle(PostResponseCommand request, CancellationToken cancellationToken)
    {
        var response = new Response(request.ResponderId, request.TargetCharacterId, request.QuestionId, request.Response) ?? throw new BadRequestException("Invalid response: missing required fields");
        await _questionResponseRepository.PostResponse(response);

        return Unit.Value;
    }
}