using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Responses.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Responses.Handlers;

public class PostResponsesHandler : IRequestHandler<PostResponsesCommand, string>
{
    private readonly IResponseRepository _responseRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly ILLMService _llmService;

    public PostResponsesHandler(
        IResponseRepository responseRepository,
        IQuestionRepository questionRepository,
        IUserRepository userRepository,
        IAgentRepository agentRepository,
        ILLMService llmService)
    {
        _responseRepository = responseRepository;
        _questionRepository = questionRepository;
        _userRepository = userRepository;
        _agentRepository = agentRepository;
        _llmService = llmService;
    }

    public async Task<string> Handle(PostResponsesCommand request, CancellationToken cancellationToken)
    {
        // check if target is a user or agent
        var isUser = await _userRepository.GetUser(request.TargetCharacterId) != null;
        if (!isUser)
        {
            _ = await _agentRepository.GetAgent(request.TargetCharacterId) ??
                throw new NotFoundException(nameof(Character), request.TargetCharacterId);
        }

        // add responses to the repository
        await _responseRepository.PostResponses(isUser, request.Responses);

        // get character summary from LLM service
        var (questionIds, responses) = await _responseRepository
            .GetQuestionIdResponsesMapping(isUser, request.TargetCharacterId);
        var questions = questionIds.Select(
            async questionId => await _questionRepository.GetQuestion(questionId)).Select(task => task.Result);
        return await _llmService.GenerateCharacterSummary(request.TargetCharacterId, questions, responses);
    }
}