using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Commands;
using SimU_GameService.Domain.Models;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class SendChatHandler : IRequestHandler<SendChatCommand, Unit>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public SendChatHandler(IChatRepository chatRepository, IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async Task<Unit> Handle(SendChatCommand request, CancellationToken cancellationToken)
    {
        var sender = await _userRepository.GetUser(request.SenderId)
            ?? throw new NotFoundException(nameof(User), request.SenderId);

        // attempt to retrieve the receiver as a User or Group
        var receiverAsUser = await _userRepository.GetUser(request.ReceiverId);
        var receiverAsGroup = receiverAsUser != null ? null : await _groupRepository.GetGroup(request.ReceiverId);

        // throw an exception if receiver is neither a User nor a Group
        if (receiverAsUser == null && receiverAsGroup == null)
        {
            throw new NotFoundException("User or Group", request.ReceiverId);
        }

        var receiver = (Entity?) receiverAsUser ?? receiverAsGroup;
        
        var chat = new Chat(request.SenderId, request.ReceiverId, request.Content, receiver is Group);
        await _chatRepository.AddChat(chat);
        return Unit.Value;
    }
}