using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class GetChatHandler : IRequestHandler<GetChatQuery, Chat?>
{
    private readonly IChatRepository _chatRepository;

    public GetChatHandler(IChatRepository chatRepository) => _chatRepository = chatRepository;

    public Task<Chat?> Handle(GetChatQuery request, CancellationToken cancellationToken) => _chatRepository.GetChat(request.ChatId);
}
