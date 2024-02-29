using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Api.Common;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Agents.Commands;
using SimU_GameService.Application.Services.Agents.Queries;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AgentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AgentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost(Name = "CreateAgent")]
    public async Task<ActionResult<IdResponse>> CreateAgent(CreateAgentRequest request)
    {
        var agentId = await _mediator.Send(new CreateAgentCommand(
            request.Username, request.Description, request.CreatorId, request.IncubationDurationInHours, request.SpriteURL, request.SpriteHeadshotURL));
        return Ok(new IdResponse(agentId));
    }

    [HttpGet("{id}", Name = "GetAgent")]
    public async Task<ActionResult<AgentResponse>> GetAgent(Guid id)
    {
        var agent = await _mediator.Send(new GetAgentQuery(id))
            ?? throw new NotFoundException(nameof(Agent), id);
        return Ok(_mapper.MapToAgentResponse(agent));
    }

    [HttpGet("{id}/summary", Name = "GetAgentSummary")]
    public async Task<ActionResult<SummaryResponse>> GetAgentSummary(Guid id)
    {
        var agentSummary = await _mediator.Send(new GetAgentSummaryQuery(id));
        return Ok(new SummaryResponse(agentSummary ?? string.Empty));
    }

    [HttpPost("description", Name = "PostVisualDescription")]
    public async Task<ActionResult<SpriteURLsResponse>> PostVisualDescription(DescriptionRequest request)
    {
        var (spriteURL, spriteHeadshotURL) = await _mediator.Send(
            new PostVisualDescriptionCommand(request.Description));
        return Ok(new SpriteURLsResponse(spriteURL, spriteHeadshotURL));
    }
}