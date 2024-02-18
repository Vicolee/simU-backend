using System.Net.Http.Json;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Characters;

public class LLMService : ILLMService
{
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly HttpClient _httpClient;

    public LLMService(IUserRepository userRepository, IChatRepository chatRepository, HttpClient httpClient)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _httpClient = httpClient;
    }

    public async Task EndConversation(Guid conversationID, IEnumerable<Guid> participants)
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateCharacterSummary(Guid characterId, IEnumerable<string> questions, IEnumerable<IEnumerable<string>> answers)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, Uri>> GenerateSprites(Guid userId, string description, Uri photo_URL)
    {
        throw new NotImplementedException();
    }

    public Task<Uri> GenerateWorldThumbnail(Guid worldId, Guid creatorId, string description)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SendChat(Guid senderId, Guid recipientId, Guid conversationID, string content,
        bool streamResponse, bool respondWithQuestion)
    {
        // check that both sender and receiver exist
        _ = await _userRepository.GetUser(senderId) ?? throw new NotFoundException(nameof(User), senderId);
        _ = await _userRepository.GetUser(recipientId) ?? throw new NotFoundException(nameof(User), recipientId);

        // confirm that we no longer need to generate the responseId like this anymore.
        Guid responseId = Guid.NewGuid();
        var request = new
        {
            senderId,
            recipientId,
            conversationID,
            content,
            streamResponse,
            respondWithQuestion
        };

        var response = await _httpClient.PostAsJsonAsync("", request);
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
}