using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class GetChatHistoryHandler : IRequestHandler<GetChatHistoryQuery, IEnumerable<Chat>>
{
    private readonly IChatRepository _chatRepository;

    public GetChatHistoryHandler(IChatRepository chatRepository) => _chatRepository = chatRepository;

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
        return await _chatRepository.GetChatsBySenderAndReceiverIds(senderId, recipientId);
    }
}