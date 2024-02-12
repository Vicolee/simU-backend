namespace SimU_GameService.Contracts.Requests;

public record ResponseRequest(Guid TargetId, Guid ResponderId, Guid QuestionId, string Response);