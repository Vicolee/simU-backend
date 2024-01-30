namespace SimU_GameService.Contracts.Responses;

public record IncubatingAgentResponse(
    Guid Id,
    DateTime HatchTime
);