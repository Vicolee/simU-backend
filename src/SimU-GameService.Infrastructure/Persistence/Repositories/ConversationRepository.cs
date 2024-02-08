using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents a repository for managing chat entities in the database.
/// </summary>
public class ConversationRepository : IConversationRepository
{
    private readonly SimUDbContext _dbContext;

    public ConversationRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task<Guid?> StartConversation(Guid senderId, Guid receiverId, bool isGroupChat = false)
    {
        var conversation = new Conversation
        {
            Participants = new List<Guid> { senderId, receiverId },
            IsGroupChat = isGroupChat
        };

        await _dbContext.Conversations.AddAsync(conversation);
        await _dbContext.SaveChangesAsync();

        return conversation.Id;
    }

    public async Task<Guid?> IsOnGoingConversation(Guid senderId, Guid receiverId)
    {
        var conversation = await _dbContext.Conversations
            .Where(c => c.Participants.Contains(senderId) && c.Participants.Contains(receiverId))
            .OrderByDescending(c => c.LastMessageTime)
            .FirstOrDefaultAsync();

        if (conversation is null)
        {
            return null;
        }

        if (conversation.IsConversationOver is true)
        {
            return null;
        }

        if (conversation.LastMessageTime < DateTime.UtcNow.AddMinutes(-15))
        {
            conversation.IsConversationOver = true;
            return null;
        }

        return conversation.Id;
    }
    public async Task<Conversation?> GetConversation(Guid conversationId) {
        return await _dbContext.Conversations
            .Where(c => c.Id == conversationId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Guid>> GetAllConversations(Guid senderId, Guid receiverId)
    {
        return await _dbContext.Conversations
            .Where(c => c.Participants.Contains(senderId) && c.Participants.Contains(receiverId))
            .Select(c => c.Id)
            .ToListAsync();
    }

    public async Task UpdateConversationLastMessageTime(Guid conversationId)
    {
        var conversation = await _dbContext.Conversations
            .Where(c => c.Id == conversationId)
            .FirstOrDefaultAsync();

        if (conversation is null)
        {
            throw new InvalidOperationException("Conversation not found");
        }

        conversation.LastMessageTime = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

}