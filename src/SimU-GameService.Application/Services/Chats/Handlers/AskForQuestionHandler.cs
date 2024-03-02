using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Common.Exceptions;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class AskForQuestionHandler : IRequestHandler<AskForQuestionQuery, Chat>
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

    public async Task<Chat> Handle(AskForQuestionQuery request, CancellationToken cancellationToken)
    {
        // sender is always a user, check that they exist
        _ = await _userRepository.GetUser(request.SenderId)
            ?? throw new NotFoundException(nameof(User), request.SenderId);

        // check if the recipient is an offline user or an agent
        var user = await _userRepository.GetUser(request.RecipientId);
        var agent = await _agentRepository.GetAgent(request.RecipientId);
       
        // recipient not found, throw exception
        if (user is null && agent is null)
        {
            throw new NotFoundException(nameof(Character), request.RecipientId);
        }

        // get existing conversation or create a new one
        var conversationId = await _conversationRepository.IsConversationOnGoing(request.SenderId, request.RecipientId)
            ?? await _conversationRepository.AddConversation(request.SenderId, request.RecipientId);
        
        // prompt LLM agent for question
        string chatResponse = await _agentService.PromptForQuestion(
            request.SenderId, request.RecipientId, conversationId, false, user != null);
        Chat question = new(
            request.RecipientId, request.SenderId, conversationId, chatResponse, false);

        // add question chat to chat repository and update conversation
        await _chatRepository.AddChat(question);
        await _conversationRepository.UpdateLastMessageSentAt(conversationId);
        return question;
    }
}
