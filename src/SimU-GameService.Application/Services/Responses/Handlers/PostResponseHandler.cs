using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Responses.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class PostResponseHandler : IRequestHandler<PostResponseCommand, string>
{
    private readonly IResponseRepository _responseRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ILLMService _llmService;

    public PostResponseHandler(IResponseRepository responseRepository, ILLMService llmService, IQuestionRepository questionRepository)
    {
        _responseRepository = responseRepository;
        _llmService = llmService;
        _questionRepository = questionRepository;
    }

    public async Task<string> Handle(PostResponseCommand request, CancellationToken cancellationToken)
    {
        var response = new Response(request.ResponderId, request.TargetCharacterId, request.QuestionId, request.Response);
        await _responseRepository.PostResponse(response);

        var (questionIds, responses) = await _responseRepository.GetQuestionIdResponsesMapping(request.TargetCharacterId);
        var questions = questionIds.Select(
            async questionId => await _questionRepository.GetQuestion(questionId)).Select(task => task.Result);

        return await _llmService.GenerateCharacterSummary(request.TargetCharacterId, questions, responses);
    }
}