using System.ComponentModel.DataAnnotations.Schema;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Agent : Character
{
    public bool IsAgent { get; set; } = true; // if we delete this IsAgent boolean, make sure to update code in SendChatHandler.cs
    public Guid CreatedByUser { get; set; }
    public int collabDurationHours { get; set; }
    public DateTime CollabEndTime { get; set;}
    public string? Description { get; set; }
    public string? Summary { get; set; }

    public bool isHatched { get; set; } = false;
    public List<QuestionResponse> QuestionResponses { get; set; }

    public Agent() : base()
    {
        QuestionResponses = new();
    }

    public Agent(
        bool isAgent,
        string username,
        Guid createdByUser,
        int collabDurationHours,
        string description,
        string summary,
        bool isHatched
        ) : this()
    {
        IsAgent = isAgent;
        Username = username;
        CreatedByUser = createdByUser;
        this.collabDurationHours = collabDurationHours;
        Description = description;
        Summary = summary;
        CalculateCollabEndTime();
        this.isHatched = isHatched;
    }

    // referenced the code for CalculateCollabEndTime() from chatGPT
    private void CalculateCollabEndTime()
    {
        CollabEndTime = CreatedTime.AddHours(collabDurationHours);
    }
}