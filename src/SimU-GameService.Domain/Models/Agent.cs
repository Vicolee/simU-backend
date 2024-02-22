namespace SimU_GameService.Domain.Models;

public class Agent : Character
{
    public Guid CreatorId { get; set; }
    public DateTime HatchTime { get; set; }
    public Uri? SpriteURL { get; set; }
    public Uri? SpriteHeadshotURL { get; set; }

    public Agent() : base()
    {
    }

    public Agent(string username,
     Guid creatorId, float collabDurationInHours, string description, Uri spriteURL, Uri spriteHeadshotUrl) : this()
    {
        Username = username;
        CreatorId = creatorId;
        Description = description;
        HatchTime = ComputeHatchTime(collabDurationInHours);
        SpriteURL = spriteURL;
        SpriteHeadshotURL = spriteHeadshotUrl;
    }
    public bool IsHatched => DateTime.UtcNow > HatchTime;

    private DateTime ComputeHatchTime(float collabDurationInHours)
        => CreatedTime.AddHours(collabDurationInHours);
}