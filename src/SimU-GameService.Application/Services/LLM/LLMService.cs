using System.Net.Http.Json;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;


namespace SimU_GameService.Application.Services;

// TODO: 
//  - review this implementation
//  - clean up the class' code
//  - abstract out this implementation (class) to Infrastructure layer
//  - retain API (interface) in Application layer

public class LLMService : ILLMService
{
    private readonly IUserRepository _userRepository;
    private readonly HttpClient _httpClient;
    private readonly IChatRepository _chatRepository;

    public LLMService(IUserRepository userRepository, HttpClient httpClient, IChatRepository chatRepository)
    {
        _userRepository = userRepository;
        _httpClient = httpClient;
        _chatRepository = chatRepository;
    }
    public async Task<Chat> RelayUserChat(Guid msgId, string msg, Guid userId, Guid agentId)
    {
        if (await _userRepository.GetUser(userId) == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        if (await _userRepository.GetUser(agentId) == null)
        {
            throw new NotFoundException(nameof(User), agentId);
        }

        var sourceAgentID = userId;
        var targetAgentID = agentId;
        var prompt = msg;
        var msgID = msgId;

        // we create a response Id ahead of time for the agent LLM to use, and then send it to them.
        Guid responseID = Guid.NewGuid();

        var request = new
            {
                sourceAgentID,
                targetAgentID,
                prompt,
                msgID,
                responseID
            };

        // The route for this LLM API call is set in the Dependency Injection file
        var response = await _httpClient.PostAsJsonAsync("", request);
        var content = await response.Content.ReadAsStringAsync();


        // Checking if the response status code is 200 OK
        var statusCode = response.StatusCode;
        if (statusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(statusCode, $"Failed to relay msg: {msgId}, with content: {msg}, from user: {userId}, to agent: {agentId}");
        }
        var agentResponse = new Chat
        {
            SenderId = agentId,
            RecipientId = userId,
            Content = content,
            IsGroupChat = false,
            Id = responseID,
            CreatedTime = DateTime.UtcNow
        };
        return agentResponse;
    }
}