using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Group : Entity
{
    public string? Name { get; set; }
    public DateTime CreatedTime { get; set; }
    private readonly List<Guid> _memberIds;

    public Group() : base()
    {
        _memberIds = new();
    }

    public Group(string name) : this()
    {
        Name = name;
        CreatedTime = DateTime.Now;
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
}