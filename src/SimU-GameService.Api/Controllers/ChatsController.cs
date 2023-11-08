using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Application.Services.Commands;
using SimU_GameService.Application.Services.Queries;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{chatId}", Name = "GetChat")]
    public async Task<ActionResult<ChatResponse>> GetChat(Guid chatId)
    {
        var result = await _mediator.Send(new GetChatQuery(chatId));
        if (result is null)
        {
            return NotFound(new { message = $"Chat with ID {chatId} not found." });
        }

        return Ok(new ChatResponse(
            result.SenderId,
            result.RecipientId,
            result.Content,
            result.IsGroupChat,
            result.CreatedTime));
    }

    [HttpDelete("{chatId}", Name = "DeleteChat")]
    public async Task<ActionResult> DeleteChat(Guid chatId)
    {
        await _mediator.Send(new DeleteChatCommand(chatId));
        return NoContent();
    }

    [HttpGet(Name = "GetChats")]
    public async Task<ActionResult<IEnumerable<ChatResponse>>> GetUserChats(
        [FromQuery] Guid senderId)
    {
        var chats = await _mediator.Send(new GetUserChatsQuery(senderId));

        if (chats is null || !chats.Any())
        {
            return NotFound(new { message = $"No chats associated with userId {senderId} found." });
        }

        var response = chats.Select(chat => new ChatResponse(
            chat.SenderId,
            chat.RecipientId,
            chat.Content,
            chat.IsGroupChat,
            chat.CreatedTime));
        return Ok(response);
    }

    [HttpGet("history", Name = "GetChatHistory")]
    public async Task<ActionResult<IEnumerable<ChatResponse>>> GetChatHistory(
        [FromQuery] Guid userA_Id, [FromQuery] Guid userB_Id)
    {
        var query = new GetChatHistoryQuery(
            userA_Id,
            userB_Id);

        var chats = await _mediator.Send(query);

        if (chats is null || !chats.Any())
        {
            return NotFound(
                new { message = $"No correspondence between users with IDs {userA_Id} and {userB_Id} found." });
        }

        return Ok(chats.Select(chat => new ChatResponse(
            chat.SenderId,
            chat.RecipientId,
            chat.Content,
            chat.IsGroupChat,
            chat.CreatedTime)));
    }
}