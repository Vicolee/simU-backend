namespace SimU_GameService.Contracts.Responses;

public record QuestionnaireResponse(
    Guid ResponderId,
    Guid QuestionId,
    string Response);