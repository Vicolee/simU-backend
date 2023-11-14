using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

public interface ILLMService
{
    Task<Chat> RelayUserChat(Guid msgId, string msg, Guid userId, Guid agentId);
    // Task<string> RelayAgentResponse(Guid msgId, string msg, Guid userId, Guid agentId);
}