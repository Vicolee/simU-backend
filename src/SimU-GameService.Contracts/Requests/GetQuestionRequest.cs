namespace SimU_GameService.Contracts.Requests;

public record AskForQuestionRequest(
    Guid SenderId,
    Guid RecipientId);