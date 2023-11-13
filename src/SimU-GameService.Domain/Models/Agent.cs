using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Agent : Entity
{
	public Guid UserId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Description { get; set; }
    public Guid AgentId { get; }
	public DateTime CreatedTime { get; set; }


	public Agent() : base()
	{
		AgentId = Id;
	}

	public Agent(Guid userID, string firstName, string lastName, string description) : this()
	{
		UserId = userID;
		FirstName = firstName;
		LastName = lastName;
		Description = description;
		CreatedTime = DateTime.UtcNow;
	}
}