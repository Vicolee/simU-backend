using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Worlds.Commands;
using SimU_GameService.Application.Services.Worlds.Queries;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorldsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorldsController(IMediator mediator) => _mediator = mediator;

    [HttpPost(Name = "CreateWorld")]
    public async Task<ActionResult<IdResponse>> CreateWorld(CreateWorldRequest request)
    {
        var worldId = await _mediator.Send(
            new CreateWorldCommand(request.Name, request.Description, request.CreatorId));
        return new IdResponse(worldId);
    }

    [HttpGet("{id}", Name = "GetWorld")]
    public async Task<ActionResult<WorldResponse>> GetWorld(Guid id)
    {
        var world = await _mediator.Send(new GetWorldQuery(id))
            ?? throw new NotFoundException(nameof(World), id);
        return Ok(MapToWorldResponse(world));
    }

    [HttpGet("{id}/creator", Name = "GetWorldCreator")]
    public async Task<ActionResult<WorldUserResponse>> GetWorldCreator(Guid id)
    {
        var creator = await _mediator.Send(new GetCreatorQuery(id)) ??
                      throw new NotFoundException("Creator of world", id);
        return Ok(MapToUserResponse(creator));
    }

    [HttpGet("{id}/users", Name = "GetWorldUsers")]
    public async Task<ActionResult<IEnumerable<WorldUserResponse>>> GetWorldUsers(Guid id)
    {
        var users = await _mediator.Send(new GetWorldUsersQuery(id));
        return Ok(users.Select(MapToUserResponse));
    }

    [HttpGet("{id}/agents", Name = "GetWorldAgents")]
    public async Task<ActionResult<IEnumerable<WorldAgentResponse>>> GetWorldAgents(Guid id)
    {
        var agents = await _mediator.Send(new GetWorldAgentsQuery(id));
        return Ok(agents.Select(MapToAgentResponse));
    }

    [Authorize]
    [HttpDelete("{id}", Name = "DeleteWorld")]
    public async Task<ActionResult> DeleteWorld(Guid id)
    {
        var identityId = User.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException("Error authorizing user");
        await _mediator.Send(new DeleteWorldCommand(id, identityId));
        return NoContent();
    }

    [HttpGet("{id}/agents/incubating", Name = "GetIncubatingAgents")]
    public async Task<ActionResult<IEnumerable<IncubatingAgentResponse>>> GetIncubatingAgents(Guid id)
    {
        var agents = await _mediator.Send(new GetIncubatingAgentsQuery(id));
        return Ok(agents.Select(MapToIncubatingAgentResponse));
    }

    [HttpGet("{id}/agents/hatched", Name = "GetHatchedAgents")]
    public async Task<ActionResult<IEnumerable<IncubatingAgentResponse>>> GetHatchedAgents(Guid id)
    {
        var agents = await _mediator.Send(new GetHatchedAgentsQuery(id));
        return Ok(agents.Select(MapToIncubatingAgentResponse));
    }

    [Authorize]
    [HttpDelete("{id}/users/{userId}", Name = "RemoveUserFromWorld")]
    public async Task<ActionResult> RemoveUserFromWorld(Guid id, Guid userId)
    {
        var identityId = User.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException("Error authorizing user");
        await _mediator.Send(new RemoveUserCommand(id, userId, identityId));
        return NoContent();
    }

    private static WorldResponse MapToWorldResponse(World world) => new
        (
            world.Id,
            world.CreatorId,
            world.Name,
            world.Description,
            world.ThumbnailURL
        );

    private static UserResponse MapToUserResponse(User creator) => new
        (
            creator.Username,
            creator.Email,
            creator.Description,
            creator.Location?.X_coord ?? default,
            creator.Location?.Y_coord ?? default,
            creator.CreatedTime
        );

    private static AgentResponse MapToAgentResponse(Agent agent) => new
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
    
    private static IncubatingAgentResponse MapToIncubatingAgentResponse(Agent agent)
        => new(agent.Id, agent.HatchTime);
}
