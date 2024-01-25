using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class GetUserChatsHandler : IRequestHandler<GetUserChatsQuery, IEnumerable<Chat>>
{
    private readonly IChatRepository _chatRepository;

    public GetUserChatsHandler(IChatRepository chatRepository) => _chatRepository = chatRepository;

    public async Task<IEnumerable<Chat>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        return await _chatRepository.GetChatsByUserId(request.UserId);
    }
}