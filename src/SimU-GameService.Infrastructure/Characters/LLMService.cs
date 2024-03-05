using System.Net.Http.Json;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;
using System.Text.Json;


namespace SimU_GameService.Infrastructure.Characters;

public class LLMService : ILLMService
{
    private readonly IUserRepository _userRepository;
    private readonly IAgentRepository _agentRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IWorldRepository _worldRepository;
    private readonly HttpClient _httpClient;

    public LLMService(IUserRepository userRepository, IChatRepository chatRepository, IAgentRepository agentRepository, IWorldRepository worldRepository, HttpClient httpClient)
    {
        _userRepository = userRepository;
        _agentRepository = agentRepository;
        _chatRepository = chatRepository;
        _worldRepository = worldRepository;
        _httpClient = httpClient;
    }

    public async Task<string> SendChat(Guid senderId, Guid recipientId, Guid conversationId, string content,
        bool streamResponse, bool isSenderUser, bool isRecipientUser)
    {
        if (isSenderUser)
        {
            // checks that the user sender exists
            _ = await _userRepository.GetUser(senderId) ?? throw new NotFoundException(nameof(User), senderId);
        }
        else
        {
            // checks that the agent sender exists
            _ = await _agentRepository.GetAgent(senderId) ?? throw new NotFoundException(nameof(Agent), senderId);
        }

        if (isRecipientUser)
        {
            // check that user receiver exists
            _ = await _userRepository.GetUser(recipientId) ?? throw new NotFoundException(nameof(User), recipientId);
        }
        else
        {
            // check that agent receiver exists
            _ = await _agentRepository.GetAgent(recipientId) ?? throw new NotFoundException(nameof(Agent), recipientId);
        }

        var request = new
        {
            senderId,
            recipientId,
            conversationId,
            content,
            streamResponse,
            isRecipientUser
        };

        var response = await _httpClient.PostAsJsonAsync("/api/agents/prompt", request);

        // TO DO: DEBUG THE STREAM RESPONSE CODE HERE
        if (streamResponse) {
            //  TO DO: store entire message in another string to save in database after
            using Stream stream = await response.Content.ReadAsStreamAsync();
            using StreamReader reader = new(stream);
            char[] buffer = new char[1024];
            int bytesRead;
            // message to store in the database
            // TO DO: GET CHAT ID FROM LLM SERVICE
            string entireMessage = "";

            while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string chunk = new string(buffer, 0, bytesRead);
                //to do: append the chatid in front of every chunk
                Console.WriteLine(chunk);
                entireMessage += chunk;
                // await _unityHub.SendChat(agentId, chunk);
            }
            // send the user a terminating character so they know that the stream is over
            // await _unityHub.SendChat(agentId, "\n");
            // TO DO: REVISE THIS!!
            var agentResponse = new Chat
            {
                SenderId = recipientId,
                RecipientId = senderId,
                ConversationId = conversationId,
                Content = entireMessage,
                IsSenderOnline = false
            };

            await _chatRepository.AddChat(agentResponse);
            // TO DO: REVISE THIS RETURN STATEMENT
            return entireMessage;
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        // throw exception if the request failed
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(
                response.StatusCode,
                $"Failed to relay message with content: {content}, from user: {senderId}, to agent: {recipientId}.");
        }
        return responseContent;
    }

    public async Task<string> PromptForQuestion(Guid senderId, Guid recipientId, Guid conversationId, bool streamResponse, bool isRecipientUser)
    {
        var request = new
        {
            senderId,
            recipientId,
            isRecipientUser,
            conversationId,
            streamResponse,
        };

        var response = await _httpClient.PostAsJsonAsync("/api/agents/question", request);
        var responseContent = await response.Content.ReadAsStringAsync();

        // throw exception if the request failed
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(
                response.StatusCode,
                $"Failed to prompt for a question from user: {senderId}, to agent: {recipientId}.");
        }
        return responseContent;
    }

    public async Task EndConversation(Guid conversationId, Guid participantA, Guid participantB)
    {

        // code checks if the recipient is an offline user or an agent.
        User? userA = null;
        Agent? agentA = null;
        User? userB = null;
        Agent? agentB = null;
        bool isParticipantUserA;
        bool isParticipantUserB;

        // check if Participant A is a user or not
        try
        {
            userA = await _userRepository.GetUser(participantA);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        try
        {
            agentA = await _agentRepository.GetAgent(participantA);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        if (userA == null && agentA == null)
        {
            throw new NotFoundException("The provided recipient Id does not exist: " + participantA);
        }
        else if (userA != null)
        {
            isParticipantUserA = true;
        }
        else
        {
            isParticipantUserA = false;
        }

        // check if Participant B is a user or not
        try
        {
            userB = await _userRepository.GetUser(participantB);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        try
        {
            agentB = await _agentRepository.GetAgent(participantB);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        if (userB == null && agentB == null)
        {
            throw new NotFoundException("The provided recipient Id does not exist: " + participantA);
        }
        else if (userB != null)
        {
            isParticipantUserB = true;
        }
        else
        {
            isParticipantUserB = false;
        }

        var request = new
        {
            conversationId,
            participantA,
            participantB,
            isParticipantUserA,
            isParticipantUserB
        };
        var response = await _httpClient.PostAsJsonAsync("/api/agents/endConversation", request);

        // throw exception if the request failed
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(
                response.StatusCode,
                $"Failed to send finished conversation with ID: {conversationId} to LLM service.");
        }
    }

    public async Task<string> GenerateCharacterSummary(Guid characterId, IEnumerable<string> questions, IEnumerable<IEnumerable<string>> answers)
    {
        User? user = null;
        Agent? agent = null;
        string? name = "";

        try
        {
            user = await _userRepository.GetUser(characterId);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        try
        {
            agent = await _agentRepository.GetAgent(characterId);
        }
        catch (NotFoundException) { /* Ignore the exception */ }

        if (user == null && agent == null)
        {
            throw new NotFoundException("Neither a user nor an agent matches the provided characterId to generate a summary: " + characterId);
        }

        if (user != null) {
            name = user.Username;
        }
        else if (agent != null) {
            name = agent.Username;
        }

        var request = new
        {
            name,
            questions,
            answers
        };

        var response = await _httpClient.PostAsJsonAsync("/api/agents/generatePersona", request);
        var responseContent = await response.Content.ReadAsStringAsync();
        // throw exception if the request failed
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(
                response.StatusCode,
                $"Failed to generate a summary for character with Id: {characterId}.");
        }
        return responseContent;
    }

    public async Task<Dictionary<string, string>?> GenerateAgentSprite(string description)
    {
        string appearanceDescription = description;
        var request = new
        {
            appearanceDescription
        };

        var response = await _httpClient.PostAsJsonAsync("/api/agents/generateAvatar", request);
        Dictionary<string, string>? responseContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        // throw exception if the request failed
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(
                response.StatusCode,
                $"Failed to generate a sprite for description: {description}.");
        }
        return responseContent;

    }

    public async Task<string> GenerateWorldThumbnail(Guid worldID, Guid creatorId, string description)
    {
        _ = await _worldRepository.GetWorld(worldID) ?? throw new NotFoundException(nameof(World), worldID);
        _ = await _userRepository.GetUser(creatorId) ?? throw new NotFoundException(nameof(User), creatorId);

        var request = new
        {
            worldID,
            creatorId,
            description
        };

        var response = await _httpClient.PostAsJsonAsync("/api/thumbnails", request);
        var responseContent = await response.Content.ReadAsStringAsync();
        // throw exception if the request failed
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(
                response.StatusCode,
                $"Failed to generate a thumbnail for world with Id: {worldID}.");
        }
        var jsonDocument = JsonDocument.Parse(responseContent);
        var thumbnailUrl = jsonDocument.RootElement.GetProperty("thumbnailURL").GetString();

        return thumbnailUrl ?? throw new ServiceErrorException(
            response.StatusCode,
            $"Failed to generate a thumbnail for world with Id: {worldID}.");
    }

    public async Task UpdateUserSummary(Guid userId, string summary)
    {
        var request = new
        {
            userId,
            summary
        };

        await _httpClient.PostAsJsonAsync("/api/agents/updateUserSummary", request);
    }
}