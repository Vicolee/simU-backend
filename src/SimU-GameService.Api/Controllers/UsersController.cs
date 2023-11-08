using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("questions", Name = "GetQuestions")]
    public async Task<ActionResult<IEnumerable<string>>> GetQuestions()
    {
        var questions = await _mediator.Send(new GetQuestionsQuery());
        return Ok(questions);
    }

    [HttpPost("{userId}/responses", Name = "PostResponses")]
    public async Task<ActionResult> PostResponses(Guid userId, IEnumerable<string> responses)
    {
        await _mediator.Send(new PostResponsesCommand(userId, responses));
        return NoContent();
    }

    [HttpGet("{userId}", Name = "GetUser")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid userId)
    {
        try
        {
            var user = await _mediator.Send(new GetUserQuery(userId))
                ?? throw new Exception($"User with ID {userId} not found.");

            return Ok(new UserResponse(
                user.FirstName,
                user.LastName,
                user.Email,
                user.LastKnownLocation?.X ?? default,
                user.LastKnownLocation?.Y ?? default,
                user.IsLoggedIn,
                user.CreatedTime,
                user.LastActiveTime));
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{userId}/friends", Name = "RemoveFriend")]
    public async Task<ActionResult> RemoveFriend(Guid userId, Guid friendId)
    {
        await _mediator.Send(new RemoveFriendCommand(userId, friendId));
        return NoContent();
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