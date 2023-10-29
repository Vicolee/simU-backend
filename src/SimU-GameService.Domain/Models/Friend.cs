namespace SimU_GameService.Domain.Models;

public record Friend
{
    public Guid FriendId { get; init; }
    public DateTime CreatedTime { get; init; }
}