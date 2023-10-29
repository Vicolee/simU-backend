namespace SimU_GameService.Domain;

public class Group
{
    public Guid Id { get; set; }
    public List<Guid> Users { get; set; }
    public string? name { get; set; }
    public DateTime createdAt { get; set; }

}