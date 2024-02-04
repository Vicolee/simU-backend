using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorldsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorldsController(IMediator mediator) => _mediator = mediator;

    [HttpPost(Name = "CreateWorld")]
    public ActionResult<IdResponse> CreateWorld(CreateWorldRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}", Name = "GetWorld")]
    public ActionResult<WorldResponse> GetWorld(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/creator", Name = "GetWorldCreator")]
    public Task<ActionResult<WorldUserResponse>> GetWorldCreator(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/users", Name = "GetWorldUsers")]
    public ActionResult<IEnumerable<WorldUserResponse>> GetWorldUsers(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/agents", Name = "GetWorldAgents")]
    public ActionResult<IEnumerable<WorldAgentResponse>> GetWorldAgents(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}", Name = "DeleteWorld")]
    public ActionResult DeleteWorld(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/agents/incubating", Name = "GetIncubatingAgents")]
    public ActionResult<IEnumerable<IncubatingAgentResponse>> GetIncubatingAgents(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/agents/hatched", Name = "GetHatchedAgents")]
    public ActionResult<IEnumerable<IncubatingAgentResponse>> GetHatchedAgents(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}/users/{userId}", Name = "RemoveUserFromWorld")]
    public ActionResult RemoveUserFromWorld(Guid id, Guid userId)
    {
        throw new NotImplementedException();
    }
}
