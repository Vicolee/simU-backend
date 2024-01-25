using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

public interface IAgentService
{
    Task<Chat> RelayUserChat(Guid chatId, string message, Guid senderId, Guid agentId);
}