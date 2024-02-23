using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents a repository for managing chat entities in the database.
/// </summary>
public class ConversationRepository : IConversationRepository
{
    private readonly SimUDbContext _dbContext;

    private readonly ILLMService LLMService;

    public ConversationRepository(SimUDbContext dbContext, ILLMService llmService)
    {
        _dbContext = dbContext;
        LLMService = llmService;
    }

    public async Task<Guid?> AddConversation(Guid senderId, Guid receiverId, bool isGroupChat = false)
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

    public async Task<Guid?> IsConversationOnGoing(Guid senderId, Guid receiverId)
    {
        var conversation = await _dbContext.Conversations
            .Where(c => c.Participants.Contains(senderId) && c.Participants.Contains(receiverId))
            .OrderByDescending(c => c.LastMessageSentAt)
            .FirstOrDefaultAsync();

        if (conversation is null)
        {
            return null;
        }

        if (conversation.IsConversationOver is true)
        {
            return null;
        }

        if (conversation.LastMessageSentAt < DateTime.UtcNow.AddMinutes(-15))
        {
            await MarkConversationAsOver(conversation.Id);
            return null;
        }
        return conversation.Id;
    }
    public async Task<Conversation?> GetConversation(Guid conversationId) {
        return await _dbContext.Conversations
            .Where(c => c.Id == conversationId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Guid>> GetConversations(Guid senderId, Guid receiverId)
    {
        return await _dbContext.Conversations
            .Where(c => c.Participants.Contains(senderId) && c.Participants.Contains(receiverId))
            .Select(c => c.Id)
            .ToListAsync();
    }

    public async Task UpdateLastMessageSentAt(Guid conversationId)
    {
        var conversation = await _dbContext.Conversations
            .Where(c => c.Id == conversationId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Conversation not found");
        conversation.LastMessageSentAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Conversation>> GetActiveConversations()
    {
        return await _dbContext.Conversations
            .Where(c => c.IsConversationOver == false)
            .ToListAsync();
    }

    public async Task MarkConversationAsOver(Guid conversationId)
    {
        var conversation = await _dbContext.Conversations
            .Where(c => c.Id == conversationId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Conversation not found");
        conversation.IsConversationOver = true;
        // make call to LLM to let them know the conversation ended.
        await LLMService.EndConversation(conversationId, conversation.Participants);
        await _dbContext.SaveChangesAsync();
    }
}