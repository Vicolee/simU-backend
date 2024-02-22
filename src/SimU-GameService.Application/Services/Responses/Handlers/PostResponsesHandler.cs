using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Responses.Commands;

namespace SimU_GameService.Application.Services.Responses.Handlers;

public class PostResponsesHandler : IRequestHandler<PostResponsesCommand, string>
{
    private readonly IResponseRepository _responseRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ILLMService _llmService;

    public PostResponsesHandler(IResponseRepository responseRepository, IQuestionRepository questionRepository, ILLMService llmService)
    {
        _responseRepository = responseRepository;
        _questionRepository = questionRepository;
        _llmService = llmService;
    }

    public async Task<string> Handle(PostResponsesCommand request, CancellationToken cancellationToken)
    {
        await _responseRepository.PostResponses(request.Responses);
        
        var (questionIds, responses) = await _responseRepository.GetQuestionIdResponsesMapping(request.TargetCharacterId);
        var questions = questionIds.Select(
            async questionId => await _questionRepository.GetQuestion(questionId)).Select(task => task.Result);

        return await _llmService.GenerateCharacterSummary(request.TargetCharacterId, questions, responses);
    }
}