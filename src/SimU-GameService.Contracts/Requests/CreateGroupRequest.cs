namespace SimU_GameService.Contracts.Requests;

public record CreateGroupRequest(Guid OwnerId, string Name);