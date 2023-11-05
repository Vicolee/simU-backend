using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    public UsersController()
    {
    }

    [HttpGet("questions", Name = "GetQuestions")]
    public Task<ActionResult<IEnumerable<string>>> GetQuestions()
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult<IEnumerable<string>>>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }

    [HttpPost("{userId}/responses", Name = "PostResponses")]
    public Task<ActionResult> PostResponses(Guid userId, IEnumerable<string> responses)
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

    [HttpGet("{userId}", Name = "GetUser")]
    public Task<ActionResult<UserResponse>> GetUser(Guid userId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult<UserResponse>>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }

    [HttpDelete("{userId}/friends", Name = "RemoveFriend")]
    public Task<ActionResult> RemoveFriend(Guid userId, Guid friendId)
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

    [HttpGet("{userId}/friends", Name = "GetFriends")]
    public Task<ActionResult<IEnumerable<FriendResponse>>> GetFriends(Guid userId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (NotImplementedException)
        {
            return Task.FromResult<ActionResult<IEnumerable<FriendResponse>>>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }
}