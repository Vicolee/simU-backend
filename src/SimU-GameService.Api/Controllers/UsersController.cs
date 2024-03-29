using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Api.Common;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{userId}", Name = "GetUser")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid userId)
    {
        var user = await _mediator.Send(new GetUserQuery(userId))
            ?? throw new NotFoundException(nameof(Domain.Models.User), userId);
        return Ok(_mapper.MapToUserResponse(user));
    }

    [HttpGet("{id}/worlds", Name = "GetUserWorlds")]
    public async Task<ActionResult<IEnumerable<WorldResponse>>> GetUserWorlds(Guid id)
    {
        var worlds = await _mediator.Send(new GetUserWorldsQuery(id));
        return Ok(worlds.Select(_mapper.MapToWorldResponse));
    }

    [HttpGet("{id}/summary", Name = "GetUserSummary")]
    public async Task<ActionResult<SummaryResponse>> GetUserSummary(Guid id)
    {
        var summary = await _mediator.Send(new GetUserSummaryQuery(id));
        return Ok(new SummaryResponse(summary ?? default!));
    }

    [HttpPut("{id}/sprite", Name = "UpdateSprite")]
    public async Task<ActionResult> UpdateSprite(Guid id, UpdateSpriteRequest request)
    {
        await _mediator.Send(new UpdateUserSpriteCommand(id, request.Animations));
        return Ok();
    }

    [HttpPut("{id}/summary", Name = "UpdateUserSummary")]
    public async Task<ActionResult> UpdateUserSummary(Guid id, UpdateSummaryRequest request)
    {
        await _mediator.Send(new UpdateUserSummaryCommand(id, request.Summary));
        return Ok();
    }
}