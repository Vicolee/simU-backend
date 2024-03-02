using SimU_GameService.Domain.Primitives;
namespace SimU_GameService.Domain.Models;

public abstract class Response : Entity
{
    public Guid ResponderId { get; set; }
    public Guid TargetId { get; set; }
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = default!;

    public Response() : base()
    {
    }

    public Response(Guid responderId, Guid targetId, Guid questionId, string response)
        : this()
    {
        ResponderId = responderId;
        TargetId = targetId;
        QuestionId = questionId;
        Content = response;
    }
}