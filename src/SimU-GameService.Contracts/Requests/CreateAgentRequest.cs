namespace SimU_GameService.Contracts.Requests;

public record CreateAgentRequest(
    string Username,
    string Description,
    Guid CreatorId,
    decimal IncubationTimeInHours);