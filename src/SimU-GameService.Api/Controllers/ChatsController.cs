using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Api.Common;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Commands;
using SimU_GameService.Application.Services.Chats.Queries;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ChatsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{chatId}", Name = "GetChat")]
    public async Task<ActionResult<ChatResponse>> GetChat(Guid chatId)
    {
        var chat = await _mediator.Send(new GetChatQuery(chatId))
            ?? throw new NotFoundException(nameof(Chat), chatId);
        return Ok(_mapper.MapToChatResponse(chat));
    }

    [Authorize]
    [HttpDelete("{id}", Name = "DeleteChat")]
    public async Task<ActionResult> DeleteChat(Guid id)
    {
        await _mediator.Send(new DeleteChatCommand(id));
        return NoContent();
    }

    [HttpGet(Name = "GetUserChats")]
    public async Task<ActionResult<IEnumerable<ChatResponse>>> GetUserChats(
        [FromQuery] Guid senderId)
    {
        var chats = await _mediator.Send(new GetUserChatsQuery(senderId));
        return Ok(chats.Select(_mapper.MapToChatResponse));
    }

    [HttpGet("history", Name = "GetChatHistory")]
    public async Task<ActionResult<IEnumerable<ChatResponse>>> GetChatHistory(
        [FromQuery] Guid participantA_Id,
        [FromQuery] Guid participantB_Id)
    {
        var query = new GetChatHistoryQuery(
            participantA_Id,
            participantB_Id);

        var chats = await _mediator.Send(query);
        return Ok(chats.Select(_mapper.MapToChatResponse));
    }

    [HttpGet("question", Name = "AskForQuestion")]
    public async Task<ActionResult<ChatResponse>> AskForQuestion(
        [FromQuery] Guid senderId,
        [FromQuery] Guid recipientId)
    {
        var question = await _mediator.Send(new AskForQuestionQuery(senderId, recipientId));

        if (question is null)
        {
            return NotFound(new { message = "No question generated, probably because the recipient had difficulty creating one." });
        }

        return Ok(question);
    }
}