namespace SimU_GameService.Contracts.Requests;

public record QuestionnaireResponseRequest(
    Guid TargetId,
    Guid ResponderId,
    IEnumerable<IdResponsePair> Responses);

public record IdResponsePair(Guid QuestionId, string Response);