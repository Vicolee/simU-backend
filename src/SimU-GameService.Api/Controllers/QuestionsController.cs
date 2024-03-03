using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Api.Common;
using SimU_GameService.Application.Services.QuestionResponses.Queries;
using SimU_GameService.Application.Services.Questions.Commands;
using SimU_GameService.Application.Services.Questions.Queries;
using SimU_GameService.Application.Services.Responses.Commands;
using SimU_GameService.Application.Services.Responses.Queries;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public QuestionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost(Name = "PostQuestion")]
    public async Task<ActionResult> PostQuestion(PostQuestionRequest request)
    {
        await _mediator.Send(new PostQuestionCommand(request.QuestionText, request.Type));
        return NoContent();
    }

    [HttpGet("users", Name = "GetUserQuestions")]
    public async Task<ActionResult<IEnumerable<QuestionResponse>>> GetUserQuestions()
    {
        var questions = await _mediator.Send(new GetUserQuestionsQuery());
        return Ok(questions.Select(_mapper.MapToQuestionResponse));
    }

    [HttpGet("agents", Name = "GetAgentQuestions")]
    public async Task<ActionResult<IEnumerable<QuestionResponse>>> GetAgentQuestions()
    {
        var questions = await _mediator.Send(new GetAgentQuestionsQuery());
        return Ok(questions.Select(_mapper.MapToQuestionResponse));
    }

    [HttpPost("responses", Name = "PostResponses")]
    public async Task<ActionResult<SummaryResponse>> PostResponses(ResponsesRequest request)
    {
        bool isUser = await _mediator.Send(new CheckIfUserQuery(request.TargetId));
        var command = _mapper.MapToPostResponsesCommand(isUser, request);
        string summary = await _mediator.Send(command);
        return Ok(summary);
    }

    [HttpPost("response", Name = "PostResponse")]
    public async Task<ActionResult<SummaryResponse>> PostResponse(ResponseRequest request)
    {
        var command = new PostResponseCommand(
            request.TargetId, request.ResponderId, request.QuestionId, request.Response);
        string summary = await _mediator.Send(command);
        return Ok(summary);
    }

    [HttpGet("responses/{targetId}", Name = "GetResponses")]
    public async Task<ActionResult<IEnumerable<AnswersResponse>>> GetResponses(Guid targetId)
    {
        var responses = await _mediator.Send(new GetResponsesQuery(targetId));
        return Ok(responses.Select(_mapper.MapToAnswersResponse));
    }

    [HttpGet("responses/{targetId}/{questionId}", Name = "GetQuestionResponses")]
    public async Task<ActionResult<IEnumerable<AnswersToQuestionResponse>>> GetQuestionResponse(Guid targetId, Guid questionId)
    {
        var responses = await _mediator.Send(new GetResponsesToQuestionQuery(targetId, questionId));
        return Ok(responses.Select(_mapper.MapToAnswersToQuestionResponse));
    }
}