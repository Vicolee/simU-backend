using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Responses.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class PostResponseHandler : IRequestHandler<PostResponseCommand, string>
{
    private readonly IResponseRepository _responseRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly ILLMService _llmService;

    public PostResponseHandler(IResponseRepository responseRepository,
        IQuestionRepository questionRepository,
        IAgentRepository agentRepository,
        IUserRepository userRepository,
        ILLMService llmService)
    {
        _responseRepository = responseRepository;
        _questionRepository = questionRepository;
        _agentRepository = agentRepository;
        _userRepository = userRepository;
        _llmService = llmService;
    }

    public async Task<string> Handle(PostResponseCommand request, CancellationToken cancellationToken)
    {
        // check if target is a user or agent
        var isUser = await _userRepository.GetUser(request.TargetCharacterId) != null;
        if (!isUser)
        {
            _ = await _agentRepository.GetAgent(request.TargetCharacterId) ??
                throw new NotFoundException(nameof(Character), request.TargetCharacterId);
        }

        Response response = isUser
            ? new UserQuestionResponse(request.ResponderId, request.TargetCharacterId, request.QuestionId, request.Response)
            : new AgentQuestionResponse(request.ResponderId, request.TargetCharacterId, request.QuestionId, request.Response);
        
        await _responseRepository.PostResponse(isUser, response);

        var (questionIds, responses) = await _responseRepository
            .GetQuestionIdResponsesMapping(isUser, request.TargetCharacterId);
        var questions = questionIds.Select(
            async questionId => await _questionRepository.GetQuestion(questionId)).Select(task => task.Result);

        return await _llmService.GenerateCharacterSummary(request.TargetCharacterId, questions, responses);
    }
}