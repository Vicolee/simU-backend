using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Handlers;

public class GetChatHistoryHandler : IRequestHandler<GetChatHistoryQuery, IEnumerable<Chat>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;

    public GetChatHistoryHandler(IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Returns the union of two chat histories where both users are the sender and recipient.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Chat>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        // compute the union of the two chat histories where both users are the sender and recipient
        var correspondenceA = await GetCorrespondence(request.UserA_Id, request.UserB_Id);
        var correspondenceB = await GetCorrespondence(request.UserB_Id, request.UserA_Id);
        return correspondenceA.Union(correspondenceB);
    }

    /// <summary>
    /// Gets a chat history from the repository by sender and recipient IDs.
    /// </summary>
    /// <param name="senderId"></param>
    /// <param name="recipientId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<IEnumerable<Chat>> GetCorrespondence(Guid senderId, Guid recipientId)
    {
        try
        {
            // get sender from database by ID
            var sender = await _userRepository.GetUserById(senderId)
                ?? throw new Exception($"Sender with ID {senderId} not found.");
    
            // get chats from database using list of chat ids from sender
            var chatsTasks = sender.ChatIds.Select(
                async chatId => await _chatRepository.GetChat(chatId)
                ?? throw new Exception($"Chat with ID {chatId} not found."));
    
            // return chats where recipient is the recipient ID
            return (await Task.WhenAll(chatsTasks))
                .Where(chat => chat.RecipientId == recipientId);
        }
        catch (System.Exception)
        {
            
            return new List<Chat>();
        }
    }
}