using System.ComponentModel.DataAnnotations.Schema;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Agent : Character
{
    // public bool IsAgent { get; set; } = true; // if we delete this IsAgent boolean, make sure to update code in SendChatHandler.cs
    public Guid CreatedByUser { get; set; }
    public DateTime CollabStartTime { get; set; }
    public DateTime CollabEndTime { get; set;}
    public string? Description { get; set; }
    public bool isHatched { get; set; } = false;
    public List<QuestionResponse> QuestionResponses { get; set; }

    public Agent() : base()
    {
        QuestionResponses = new();
    }

    public Agent(
        string username,
        Guid createdByUser,
        float collabDurationHours,
        string description
        ) : this()
    {
        Username = username;
        CreatedByUser = createdByUser;
        CollabStartTime = DateTime.UtcNow;
        Description = description;
        CalculateCollabEndTime(collabDurationHours);
    }

    private void CalculateCollabEndTime(float collabDurationHours)
    {
        CollabEndTime = CreatedTime.AddHours(collabDurationHours);
    }
}