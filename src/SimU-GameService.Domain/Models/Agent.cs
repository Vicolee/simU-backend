namespace SimU_GameService.Domain.Models;

public class Agent : Character
{
    public Guid CreatorId { get; set; }
    public DateTime HatchTime { get; set; }

    public Agent() : base()
    {
    }

    public Agent(string username,
     Guid createdByUser, float collabDurationInHours, string description) : this()
    {
        Username = username;
        CreatorId = createdByUser;
        Description = description;
        HatchTime = ComputeHatchTime(collabDurationInHours);
    }
    
    public bool IsHatched => DateTime.UtcNow > HatchTime;

    private DateTime ComputeHatchTime(float collabDurationInHours)
        => CreatedTime.AddHours(collabDurationInHours);
}