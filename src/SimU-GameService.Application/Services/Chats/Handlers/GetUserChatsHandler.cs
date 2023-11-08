using MediatR;
using SimU_GameService.Application.Common.Abstractions;
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
        try
        {
            // get user from database
            var user = await _userRepository.GetUserById(request.UserId) ?? throw new Exception("User not found.");

            // get chats from database using list of chat ids from user
            var chats = user.ChatIds.Select(
                async chatId => await _chatRepository.GetChat(chatId)
                ?? throw new Exception($"Chat with ID {chatId} not found."));
            return await Task.WhenAll(chats);
        }
        catch (Exception)
        {

            return new List<Chat>();
        }
    }
}