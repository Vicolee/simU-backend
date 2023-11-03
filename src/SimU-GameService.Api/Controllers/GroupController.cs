using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    public GroupsController()
    {
    }

    [HttpPost(Name = "CreateGroup")]
    public Task<ActionResult<CreateGroupResponse>> CreateGroup(CreateGroupRequest request)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult<CreateGroupResponse>>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }

    [HttpDelete("{groupId}", Name = "DeleteGroup")]
    public Task<ActionResult> DeleteGroup(Guid groupId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }
}