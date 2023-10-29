namespace SimU_GameService.Domain.Models;

public class Group
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public DateTime CreatedTime { get; set; }
    private readonly List<Guid> _memberIds;

    public Group(string name)
    {
        Name = name;
        CreatedTime = DateTime.Now;
        _memberIds = new();
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