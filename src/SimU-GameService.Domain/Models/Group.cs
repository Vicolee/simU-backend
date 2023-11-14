using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Group : Entity
{
    public string? Name { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedTime { get; set; }
    private readonly List<Guid> _memberIds = new();

    public Group() : base()
    {
        GroupId = Id;
    }

    public Group(string name, Guid ownerId) : this()
    {
        Name = name;
        OwnerId = ownerId;
        CreatedTime = DateTime.UtcNow;

        // add owner to list of members
        _memberIds.Add(ownerId);
    }

    public void AddUser(Guid userId)
    {
        _memberIds.Add(userId);
    }

    public void RemoveUser(Guid userId)
    {
        _memberIds.Remove(userId);
    }

    public List<Guid> MemberIds => _memberIds;

    public Guid GroupId { get; }
}