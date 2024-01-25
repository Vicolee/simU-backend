using System.Net.Http.Json;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Agents;

public class AgentService : IAgentService
{
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly HttpClient _httpClient;

    public AgentService(IUserRepository userRepository, IChatRepository chatRepository, HttpClient httpClient)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _httpClient = httpClient;
    }
    public async Task<Chat> RelayUserChat(Guid chatId, string message, Guid senderId, Guid agentId)
    {
        // check that both sender and receiver exist
        _ = await _userRepository.GetUser(senderId) ?? throw new NotFoundException(nameof(User), senderId);
        _ = await _userRepository.GetUser(agentId) ?? throw new NotFoundException(nameof(User), agentId);

        Guid responseId = Guid.NewGuid();
        var request = new
        {
            senderId,
            agentId,
            message,
            chatId,
            responseId
        };

        var response = await _httpClient.PostAsJsonAsync("", request);
        var content = await response.Content.ReadAsStringAsync();

        // throw exception if the request failed
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(
                response.StatusCode,
                $"Failed to relay msg: {chatId}, with content: {message}, from user: {senderId}, to agent: {agentId}");
        }

        // return the agent's response as a Chat object
        var agentResponse = new Chat
        {
            SenderId = agentId,
            RecipientId = senderId,
            Content = content,
            IsGroupChat = false,
            Id = responseId,
            CreatedTime = DateTime.UtcNow
        };
        return agentResponse;
    }
}