namespace SimU_GameService.Application.Common.Abstractions;

public interface ILLMService
{
    Task<string> RelayUserChat(Guid msgId, string msg, Guid userId, Guid agentId);
    // Task<string> RelayAgentResponse(Guid msgId, string msg, Guid userId, Guid agentId);
}