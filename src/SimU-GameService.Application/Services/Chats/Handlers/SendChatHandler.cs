using MediatR;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class SendChatHandler : IRequestHandler<SendChatCommand, (Chat, Chat?)>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly ILLMService _agentService;
    private readonly IConversationRepository _conversationRepository;

    public SendChatHandler(IChatRepository chatRepository,
        IUserRepository userRepository,
        IAgentRepository agentRepository,
        ILLMService agentService,
        IConversationRepository conversationRepository)
    {
        _chatRepository = chatRepository;
        _agentRepository = agentRepository;
        _userRepository = userRepository;
        _agentService = agentService;
        _conversationRepository = conversationRepository;
    }

    public async Task<(Chat, Chat?)> Handle(SendChatCommand request, CancellationToken cancellationToken)
    {
        // retrieve sender and receiver as User or Agent
        Character? sender = await _userRepository.GetUser(request.SenderId);
        Character? receiver = await _userRepository.GetUser(request.ReceiverId);

        bool senderIsUser = sender is not null;
        bool receiverIsUser = receiver is not null;

        if (!senderIsUser)
        {
            sender = await _agentRepository.GetAgent(request.SenderId)
                ?? throw new NotFoundException(nameof(Character), request.SenderId);
        }
        if (!receiverIsUser)
        {
            receiver = await _agentRepository.GetAgent(request.ReceiverId)
                ?? throw new NotFoundException(nameof(Character), request.ReceiverId);
        }

        // check if user sender and/or recipients are online
        bool senderIsOnlineUser = sender is User senderAsUser && senderAsUser.IsOnline;
        bool receiverIsOnlineUser = receiver is User receiverAsUser && receiverAsUser.IsOnline;

        // fetch ongoing conversation or start a new one
        var conversationId = await _conversationRepository.IsConversationOnGoing(request.SenderId, request.ReceiverId)
            ?? await _conversationRepository.AddConversation(request.SenderId, request.ReceiverId);

        // add chat to chats and conversation repositories
        var chat = new Chat(request.SenderId, request.ReceiverId, conversationId, request.Content, senderIsOnlineUser);
        await _chatRepository.AddChat(chat);
        await _conversationRepository.UpdateLastMessageSentAt(conversationId);

        // send the chat to the LLM agent if the recipient is an agent or an offline user
        string? response = default;
        if (!receiverIsUser)
        {
            response = await _agentService.SendChat(
                chat.SenderId, chat.RecipientId, chat.ConversationId, chat.Content, false, senderIsUser, false);
        }
        else if (!receiverIsOnlineUser)
        {
            response = await _agentService.SendChat(
                chat.SenderId, chat.RecipientId, chat.ConversationId, chat.Content, false, senderIsUser, true);
        }

        // save chat response to database and return it to be sent to the original sender
        if (response is not null)
        {
            var chatResponse = new Chat(chat.RecipientId, chat.SenderId, chat.ConversationId, response, false);
            await _chatRepository.AddChat(chatResponse);
            return (chat, chatResponse);
        }

        // no response is expected here (the recipient is an online user)
        return (chat, null);
    }
}