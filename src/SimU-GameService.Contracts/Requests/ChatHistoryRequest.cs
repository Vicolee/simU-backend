namespace SimU_GameService.Contracts.Requests;
public record ChatHistoryRequest(Guid senderId, Guid recipientId);