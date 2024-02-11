using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Commands;
using SimU_GameService.Domain.Models;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class SendChatHandler : IRequestHandler<SendChatCommand, Chat>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IConversationRepository _conversationRepository;

    private readonly ILLMService _llmService;

    public SendChatHandler(IChatRepository chatRepository, IUserRepository userRepository, IAgentRepository agentRepository, IGroupRepository groupRepository, ILLMService llmService)
    {
        _chatRepository = chatRepository;
        _agentRepository = agentRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _llmService = llmService;
    }

    public async Task<Chat> Handle(SendChatCommand request, CancellationToken cancellationToken)
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
        if (receiverAsUser != null)
        {
            receiver = receiverAsUser;
        }
        else if (receiverAsAgent != null)
        {
            receiver = receiverAsAgent;
        }
        else
        {
            receiver = receiverAsGroup;
        }

        // TO DO: LEKINA DOUBLE CHECK LOGIC OF THIS CODE
        var conversationId = await _conversationRepository.IsOnGoingConversation(request.SenderId, request.ReceiverId);
        if (conversationId == null)
        {
            // this is start of a new conversation
            conversationId = await _conversationRepository.StartConversation(request.SenderId, request.ReceiverId);
        }

        Chat chat;
        if (senderIsUser) {
            // if the sender is user, checks whether user was online or offline
            // when message was sent (sender.IsOnline)
            User senderAsUser = (User) sender;
            chat = new Chat(request.SenderId, request.ReceiverId, conversationId.Value, request.Content, senderAsUser.IsOnline, receiver is Group, null, DateTime.UtcNow);
        } else {
            // sender is agent in this case
            chat = new Chat(request.SenderId, request.ReceiverId, conversationId.Value, request.Content, false, receiver is Group, null, DateTime.UtcNow);
        }

        await _chatRepository.AddChat(chat);
        await _conversationRepository.UpdateConversationLastMessageTime(conversationId.Value);

        if (receiverAsAgent != null)
        {
            // the receiver of the chat is an LLM agent, so we send the chat to the agent and await its response
            var chatResponse = await _llmService.RelayUserChat(chat.Id, chat.Content, chat.SenderId, chat.RecipientId);
            await _chatRepository.AddChat(chatResponse);
            return chatResponse;
        } else {
            return chat;
        }
    }
}