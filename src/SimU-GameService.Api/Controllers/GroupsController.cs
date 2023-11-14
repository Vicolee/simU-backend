using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;
    public GroupsController(IMediator mediator) => _mediator = mediator;

    [HttpPost(Name = "CreateGroup")]
    public async Task<ActionResult<CreateGroupResponse>> CreateGroup(CreateGroupRequest request)
    {
        var groupId = await _mediator.Send(new CreateGroupCommand(request.Name, request.OwnerId));
        return new CreateGroupResponse(groupId);  
    }

    [HttpDelete("{groupId}", Name = "DeleteGroup")]
    public async Task<ActionResult> DeleteGroup(Guid groupId)
    {
        await _mediator.Send(new DeleteGroupCommand(groupId));
        return NoContent();
    }
}