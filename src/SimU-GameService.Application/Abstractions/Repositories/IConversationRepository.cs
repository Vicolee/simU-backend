using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

public interface IConversationRepository
{
    Task<Guid?> StartConversation(Guid senderId, Guid receiverId, bool isGroupChat = false);
    Task<Guid?> IsOnGoingConversation(Guid senderId, Guid receiverId);
    Task<Conversation?> GetConversation(Guid conversationId);
    Task<IEnumerable<Guid>> GetAllConversations(Guid senderId, Guid receiverId);
    Task UpdateConversationLastMessageTime(Guid conversationId);
}