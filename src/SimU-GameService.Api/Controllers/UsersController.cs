using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Common.Exceptions;
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

        var user = await _mediator.Send(new GetUserQuery(userId))
            ?? throw new NotFoundException(nameof(Domain.Models.User), userId);
        // Console.WriteLine("here is the user's location id: ", user.Location?.LocationId);
        // var location = user.Location != null ? await _mediator.Send(new GetLocationQuery(user.Location.LocationId)) : null;
        return MapUserToUserResponse(user);
        // return MapUserToUserResponse(user, location ?? new Location());
    }

    // private ActionResult<UserResponse> MapUserToUserResponse(User user, Location location)
    private ActionResult<UserResponse> MapUserToUserResponse(User user)
    {
        return Ok(new UserResponse(
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Description,
                    user.Location?.X ?? default,
                    user.Location?.Y ?? default,
                    user.CreatedTime));
    }

    [HttpDelete("{userId}/friends", Name = "RemoveFriend")]
    public async Task<ActionResult> RemoveFriend(Guid userId, Guid friendId)
    {
        await _mediator.Send(new RemoveFriendCommand(userId, friendId));
        return NoContent();
    }

    [HttpGet("{userId}/friends", Name = "GetFriends")]
    public async Task<ActionResult<IEnumerable<FriendResponse>>> GetFriends(Guid userId)
    {
        var friends = await _mediator.Send(new GetFriendsQuery(userId));
        return Ok(friends.Select(f => new FriendResponse(f.FriendId, f.CreatedTime)));
    }


}