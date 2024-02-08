using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Services;

public interface IAgentService
{
    Task<Chat> RelayUserChat(Guid chatId, string message, Guid senderId, Guid agentId);
}