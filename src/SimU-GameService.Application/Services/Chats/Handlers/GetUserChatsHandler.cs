using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class GetUserChatsHandler : IRequestHandler<GetUserChatsQuery, IEnumerable<Chat>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public GetUserChatsHandler(IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Chat>> Handle(GetUserChatsQuery request, CancellationToken cancellationToken)
    {
        return await _chatRepository.GetChatsByUserId(request.UserId);
    }
}