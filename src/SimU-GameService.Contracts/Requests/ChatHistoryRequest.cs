namespace SimU_GameService.Contracts.Requests;

public record ChatHistoryRequest(Guid ParticipantA_Id, Guid ParticipantB_Id);
