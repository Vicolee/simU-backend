using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly SimUDbContext _dbContext;

    public ChatRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task AddChat(Chat chat)
    {
        await _dbContext.Chats.AddAsync(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteChat(Guid chatId)
    {
        var chat = await _dbContext.Chats
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat is null)
        {
            return;
        }
        _dbContext.Chats.Remove(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Chat?> GetChat(Guid chatId) => await _dbContext.Chats
            .FirstOrDefaultAsync(c => c.Id == chatId);
}