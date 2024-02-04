using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("users", Name = "GetUserQuestions")]
    public Task<ActionResult<IEnumerable<QuestionResponse>>> GetUserQuestions()
    {
        throw new NotImplementedException();
    }

    [HttpGet("agents", Name = "GetAgentQuestions")]
    public Task<ActionResult<IEnumerable<QuestionResponse>>> GetAgentQuestions()
    {
        throw new NotImplementedException();
    }

    [HttpPost("responses", Name = "PostResponses")]
    public Task<ActionResult> PostResponses(QuestionnaireResponseRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("responses/{id}", Name = "GetResponses")]
    public Task<ActionResult<IEnumerable<QuestionnaireResponse>>> GetResponses(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("responses/{id}/{questionId}", Name = "GetQuestionResponse")]
    public Task<ActionResult<IEnumerable<AnswerResponse>>> GetQuestionResponse(Guid id, Guid questionId)
    {
        throw new NotImplementedException();
    }
}