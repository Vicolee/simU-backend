namespace SimU_GameService.Contracts.Responses;

public record AnswersResponse(Guid ResponderId, Guid QuestionId, string Response);