using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class GetChatHandler : IRequestHandler<GetChatQuery, Chat?>
{
    private readonly IChatRepository _chatRepository;

    public GetChatHandler(IChatRepository chatRepository) => _chatRepository = chatRepository;

    public Task<Chat?> Handle(GetChatQuery request, CancellationToken cancellationToken)
    {
        // get chat from database
        var chat = _chatRepository.GetChat(request.ChatId)
                   ?? throw new Exception($"Chat with ID {request.ChatId} not found.");
        return chat;
    }
}
