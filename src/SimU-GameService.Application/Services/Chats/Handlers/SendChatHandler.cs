using MediatR;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Commands;
using SimU_GameService.Domain.Models;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class SendChatHandler : IRequestHandler<SendChatCommand, Chat?>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ILLMService _agentService;
    private readonly IConversationRepository _conversationRepository;

    public SendChatHandler(IChatRepository chatRepository, IUserRepository userRepository, IAgentRepository agentRepository, IGroupRepository groupRepository, ILLMService agentService, IConversationRepository conversationRepository)
    {
        _chatRepository = chatRepository;
        _agentRepository = agentRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _agentService = agentService;
        _conversationRepository = conversationRepository;
    }

    public async Task<Chat?> Handle(SendChatCommand request, CancellationToken cancellationToken)
    {
        bool senderIsUser;
        Character? sender = await _userRepository.GetUser(request.SenderId);

        if (sender != null)
        {
            senderIsUser = true;
        }
        else
        {
            sender = await _agentRepository.GetAgent(request.SenderId);
            senderIsUser = false;
        }

        if (sender == null)
        {
            throw new NotFoundException(nameof(Character), request.SenderId);
        }

        // attempt to retrieve the receiver as a User or Group
        var receiverAsUser = await _userRepository.GetUser(request.ReceiverId);
        var receiverAsAgent = receiverAsUser != null ? null : await _agentRepository.GetAgent(request.ReceiverId);
        var receiverAsGroup = receiverAsUser != null || receiverAsAgent != null ? null : await _groupRepository.GetGroup(request.ReceiverId);

        // throw an exception if receiver is neither a User nor a Group
        if (receiverAsUser == null && receiverAsAgent == null && receiverAsGroup == null)
        {
            throw new NotFoundException("User, Agent, or Group", request.ReceiverId);
        }

        Entity? receiver;
        bool receiverIsOfflineUser = false;
        if (receiverAsUser != null)
        {
            receiver = receiverAsUser;
            if (!receiverAsUser.IsOnline)
            {
                // if the receiver is not online, the AI Service must respond for them
                receiverIsOfflineUser = true;
            }
        }
        else if (receiverAsAgent != null)
        {
            receiver = receiverAsAgent;
        }
        else
        {
            receiver = receiverAsGroup;
        }

        var conversationId = await _conversationRepository.IsConversationOnGoing(request.SenderId, request.ReceiverId) ?? await _conversationRepository.AddConversation(request.SenderId, request.ReceiverId);
        Chat chat;

        if (senderIsUser) {
            // if the sender is user, checks whether user was online or offline when message was sent (sender.IsOnline)
            User senderAsUser = (User) sender;
            chat = new Chat(request.SenderId, request.ReceiverId, conversationId.Value, request.Content, senderAsUser.IsOnline, receiver is Group);
        } else {
            // sender is agent in this case
            chat = new Chat(request.SenderId, request.ReceiverId, conversationId.Value, request.Content, false, receiver is Group);
        }

        await _chatRepository.AddChat(chat);

        await _conversationRepository.UpdateLastMessageSentAt(conversationId.Value);

        if (receiverAsAgent != null || receiverIsOfflineUser)
        {
            var AIResponse = "";
            // send the chat to the LLM agent and save its response
            if (receiverIsOfflineUser) {
                // if the recipient is an offline user, we make the last parameter "isRecipientUser" = true.
                AIResponse = await _agentService.SendChat(
                    chat.SenderId, chat.RecipientId, chat.ConversationId, chat.Content, false, senderIsUser, true);
            } else {
                // if the recipient is an agent, we make the last parameter "isRecipientUser" = false.
                AIResponse = await _agentService.SendChat(
                    chat.SenderId, chat.RecipientId, chat.ConversationId, chat.Content, false, senderIsUser, false);
            }

            // TODO: create chat object from string response and save it to the database
            var chatResponse = new Chat(chat.RecipientId, chat.SenderId, chat.ConversationId, AIResponse, false, false);
            await _chatRepository.AddChat(chatResponse);

            // is this return value correct? Should we not be returning the chat response?
            return chatResponse;
        } else {
            // review this: what should we return in the case where the chat is just sent to another online user?
            return chat;
        }
    }
}