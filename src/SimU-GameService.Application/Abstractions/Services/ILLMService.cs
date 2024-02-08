namespace SimU_GameService.Application.Abstractions.Services;

public interface ILLMService
{
    Task<string> SendChat(Guid senderId, Guid recipientId, Guid conversationID, string content, bool streamResponse,
                          bool respondWithQuestion);
    Task EndConversation(Guid conversationID, IEnumerable<Guid> participants);
    Task<Dictionary<string, Uri>> GenerateSprites(Guid userId, string description, Uri photo_URL);
    Task<string> GenerateCharacterSummary(Guid characterId, IEnumerable<string> questions,
                                          IEnumerable<IEnumerable<string>> answers);
    Task<Uri> GenerateWorldThumbnail(Guid worldId, Guid creatorId, string description);
}