namespace SimU_GameService.Contracts.Requests;

public record class CreateGroupRequest(Guid OwnerId, string Name);