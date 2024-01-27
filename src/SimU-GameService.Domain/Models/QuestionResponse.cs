using SimU_GameService.Domain.Primitives;
namespace SimU_GameService.Domain.Models;

public class QuestionResponse : Entity
{
    public Guid ResponderId { get; set; } // could be either user or agent responding

    // IMPORTANT: This is the character that the responder is writing about. If a user is answering questions about themselves, the responderId and TargetCharacterId will be the same.
    public Guid TargetCharacterId { get; set; }
    public Guid QuestionId { get; set; }
    public string? Response { get; set; }

    public QuestionResponse() : base() {}

    public QuestionResponse(
        Guid responderId,
        Guid targetCharacterId,
        Guid questionId,
        string response
        ) : this()
    {
        ResponderId = responderId;
        TargetCharacterId = targetCharacterId;
        QuestionId = questionId;
        Response = response;
    }
}