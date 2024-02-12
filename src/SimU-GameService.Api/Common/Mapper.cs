using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Common;

public class Mapper : IMapper
{
    public WorldResponse MapToWorldResponse(World world) => new
        (
            world.Id,
            world.CreatorId,
            world.Name,
            world.Description,
            world.ThumbnailURL
        );

    public UserResponse MapToUserResponse(User creator) => new
        (
            creator.Username,
            creator.Email,
            creator.Description,
            creator.Location?.X_coord ?? default,
            creator.Location?.Y_coord ?? default,
            creator.CreatedTime
        );

    public AgentResponse MapToAgentResponse(Agent agent) => new
        (
            agent.Id,
            agent.Username,
            agent.Description,
            agent.Summary,
            agent.Location?.X_coord ?? default,
            agent.Location?.Y_coord ?? default,
            agent.CreatorId,
            agent.IsHatched,
            agent.SpriteURL,
            agent.SpriteHeadshotURL,
            agent.CreatedTime,
            agent.HatchTime
        );

    public IncubatingAgentResponse MapToIncubatingAgentResponse(Agent agent)
        => new(agent.Id, agent.HatchTime);
}

public interface IMapper
{
    WorldResponse MapToWorldResponse(World world);
    UserResponse MapToUserResponse(User creator);
    AgentResponse MapToAgentResponse(Agent agent);
    IncubatingAgentResponse MapToIncubatingAgentResponse(Agent agent);
}