namespace SimU_GameService.Domain.Models;

public class Agent : Character
{
    public Guid CreatorId { get; set; }
    public string? Description { get; set; }
    public DateTime HatchTime { get; set; }
    public Uri? SpriteURL { get; set; }
    public Uri? SpriteHeadshotURL { get; set; }

    public Agent() : base()
    {
    }

    public Agent(string username,
     Guid creatorId, float collabDurationInHours, string description, Uri spriteURL, Uri spriteHeadshotUrl) : base(username, string.Empty)
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
    {
        return DateTime.SpecifyKind(CreatedTime.AddHours(collabDurationInHours), DateTimeKind.Utc);
    }
}
