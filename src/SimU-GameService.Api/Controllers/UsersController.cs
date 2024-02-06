using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{userId}", Name = "GetUser")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid userId)
    {
        var user = await _mediator.Send(new GetUserQuery(userId))
            ?? throw new NotFoundException(nameof(Domain.Models.User), userId);
        return MapUserToUserResponse(user);
    }
    
    private ActionResult<UserResponse> MapUserToUserResponse(User user)
    {
        return Ok(new UserResponse(
                    user.Username,
                    user.Email,
                    user.Description,
                    user.Location?.X_coord ?? default,
                    user.Location?.Y_coord ?? default,
                    user.CreatedTime));
    }
    
    [Authorize]
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

    [HttpGet("{id}/worlds", Name = "GetUserWorlds")]
    public Task<ActionResult<IEnumerable<WorldResponse>>> GetUserWorlds(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}/worlds/{worldId}", Name = "AddUserWorld")]
    public Task<ActionResult> AddUserWorld(Guid id, Guid worldId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}/worlds/{worldId}", Name = "RemoveUserWorld")]
    public Task<ActionResult> RemoveUserWorld(Guid id, Guid worldId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id}/sprite", Name = "UpdateSprite")]
    public Task<ActionResult> UpdateSprite(UpdateSpriteRequest request)
    {
        throw new NotImplementedException();
    }
}