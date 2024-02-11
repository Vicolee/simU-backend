using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AgentsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AgentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost(Name = "CreateAgent")]
    public Task<ActionResult<IdResponse>> CreateAgent(CreateAgentRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}", Name = "GetAgent")]
    public Task<ActionResult<AgentResponse>> GetAgent(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}/summary", Name = "GetAgentSummary")]
    public Task<ActionResult<AgentSummaryResponse>> GetAgentSummary(Guid id)
    {
        throw new NotImplementedException();
    }
}