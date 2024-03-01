using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Common.Exceptions;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class AskForQuestionHandler : IRequestHandler<AskForQuestionQuery, string?>
{
    private readonly IChatRepository _chatRepository;
    private readonly ILLMService _agentService;
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;

    public AskForQuestionHandler(IChatRepository chatRepository, ILLMService agentService, IConversationRepository conversationRepository, IUserRepository userRepository, IAgentRepository agentRepository)
    {
        _chatRepository = chatRepository;
        _agentService = agentService;
        _conversationRepository = conversationRepository;
        _userRepository = userRepository;
        _agentRepository = agentRepository;
    }

    public async Task<string?> Handle(AskForQuestionQuery request, CancellationToken cancellationToken)
    {
        // sender is always a user, check that they exist
        _ = await _userRepository.GetUser(request.SenderId) ?? throw new NotFoundException(nameof(User), request.SenderId);

        // code checks if the recipient is an offline user or an agent.
        User? user = null;
        Agent? agent = null;
        bool isRecipientUser;

        try
        {
            user = await _userRepository.GetUser(request.RecipientId);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        try
        {
            agent = await _agentRepository.GetAgent(request.RecipientId);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        if (user == null && agent == null)
        {
            throw new NotFoundException("The provided recipient Id does not exist: " + request.RecipientId);
        }
        else if (user != null)
        {
            isRecipientUser = true;
        }
        else
        {
            isRecipientUser = false;
        }

        var conversationId = await _conversationRepository.IsConversationOnGoing(request.SenderId, request.RecipientId)
            ?? await _conversationRepository.AddConversation(request.SenderId, request.RecipientId);
        string chatResponse = await _agentService.PromptForQuestion(request.SenderId, request.RecipientId, conversationId, false, isRecipientUser);
        Chat question = new(request.RecipientId, request.SenderId, conversationId, chatResponse, false);
        await _chatRepository.AddChat(question);
        await _conversationRepository.UpdateLastMessageSentAt(conversationId);
        return question.Content;
    }
}
