using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

public interface IConversationRepository
{
    Task<Guid?> AddConversation(Guid senderId, Guid receiverId, bool isGroupChat = false);
    Task<Guid?> IsConversationOnGoing(Guid senderId, Guid receiverId);
    Task<Conversation?> GetConversation(Guid conversationId);
    Task<IEnumerable<Guid>> GetConversations(Guid senderId, Guid receiverId);
    Task UpdateLastMessageTime(Guid conversationId);
}