using Microsoft.AspNetCore.Mvc;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatsController : ControllerBase
{
    public ChatsController()
    {
    }

    [HttpGet("{chatId}", Name = "GetChat")]
    public Task<ActionResult<ChatResponse>> GetChat(Guid chatId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult<ChatResponse>>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }

    [HttpDelete("{chatId}", Name = "DeleteChat")]
    public Task<ActionResult> DeleteChat(Guid chatId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }

    [HttpGet(Name = "GetChats")]
    public Task<ActionResult<IEnumerable<ChatResponse>>> GetUserChats(
        [FromQuery] Guid senderId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult<IEnumerable<ChatResponse>>>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }

    [HttpGet("history", Name = "GetChatHistory")]
    public Task<ActionResult<IEnumerable<ChatResponse>>> GetChatHistory(
        ChatHistoryRequest request)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch
        {
            return Task.FromResult<ActionResult<IEnumerable<ChatResponse>>>(
                StatusCode(501, new { message = "This endpoint is not yet implemented." }));
        }
    }
}