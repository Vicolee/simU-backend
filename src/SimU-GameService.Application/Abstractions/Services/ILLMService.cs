namespace SimU_GameService.Application.Abstractions.Services;

public interface ILLMService
{
    Task<string> SendChat(Guid senderId, Guid recipientId, Guid conversationID, string content, bool streamResponse,
                          bool isSenderUser, bool isRecipientUser);
    Task<string> PromptForQuestion(Guid senderId, Guid recipientId, Guid conversationID, bool streamResponse, bool isRecipientUser);
    Task EndConversation(Guid conversationID, Guid participantA, Guid participantB);
    Task<string> GenerateCharacterSummary(Guid characterId, IEnumerable<string> questions,
                                          IEnumerable<IEnumerable<string>> answers);
    Task<Dictionary<string, string>?> GenerateAgentSprite(string description);
    Task<string> GenerateWorldThumbnail(Guid worldId, Guid creatorId, string description);
}