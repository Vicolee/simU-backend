using MediatR;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SimU_GameService.Api.Hubs.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Commands;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;
using SimU_GameService.Api.Common;
using SimU_GameService.Api.OnlineStatus;
using SimU_GameService.Application.Services.Worlds.Queries;
using SimU_GameService.Api.DomainEvents.Events;

namespace SimU_GameService.Api.Hubs;

/// <summary>
/// This class defines the methods on the server that can be invoked by the SignalR Unity client.
/// </summary>
[SignalRHub]
public class UnityHub : Hub<IUnityClient>, IUnityServer
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ConnectionService _connectionService;

    public UnityHub(IMediator mediator, IMapper mapper, ConnectionService connectionService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _connectionService = connectionService;
    }

    private string? GetIdentityIdFromConnectionMap()
        => _connectionService.GetIdentityIdFromConnectionId(Context.ConnectionId);

    private async Task<Guid> GetUserIdFromIdentityId(string identityId)
        => await _mediator.Send(new GetUserIdFromIdentityIdQuery(identityId));

    private async Task<string> GetIdentityIdFromUserId(Guid userId)
        => await _mediator.Send(new GetIdentityIdFromUserIdQuery(userId))
        ?? throw new NotFoundException(nameof(User), userId);

    public override async Task OnConnectedAsync()
    {
        var identityId = (Context.User?.FindFirst("user_id")?.Value)
            ?? throw new UnauthorizedAccessException("User not authenticated");        
        _connectionService.AddConnection(identityId, Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionService.RemoveConnectionByConnectionId(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendChat(Guid receiverId, string message)
    {
        // save chat to database and send chat to LLM agent if the receiver is an agent
        var senderIdentityId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        var (chat, response) = await _mediator.Send(
            new SendChatCommand(await GetUserIdFromIdentityId(senderIdentityId), receiverId, message));

        // send chat to receiver
        var receiverIdentityId = await GetIdentityIdFromUserId(receiverId);
        var connectionId = _connectionService.GetConnectionId(receiverIdentityId);
        if (receiverIdentityId is not null && connectionId is not null)
        {
            await Clients.Client(connectionId).ChatHandler(_mapper.MapToChatResponse(chat));
        }

        // send chat back to sender
        await Clients.Caller.ChatHandler(_mapper.MapToChatResponse(chat));

        // send agent response to sender
        if (response is not null)
        {
            await Clients.Caller.ChatHandler(_mapper.MapToChatResponse(response));
        }
    }

    public async Task UpdateLocation(Location location)
    {
        var identityId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        var senderId = await GetUserIdFromIdentityId(identityId);
        _ = await _mediator.Send(new UpdateLocationCommand(
            senderId, location.X_coord, location.Y_coord));

        // broadcast location update to all users
        await Clients.All.UpdateLocationHandler(senderId, location);
    }

    public void PingServer()
    {
        var identityId = GetIdentityIdFromConnectionMap()
            ?? throw new NotFoundException(nameof(User), Context.ConnectionId);
        _connectionService.UpdateLastPingTime(identityId);
    }

    private async Task<string> CreateSameWorldUsersGroup(Guid userId, CancellationToken cancellationToken)
    {
        var (worldName, worldUserIdentities) = await _mediator.Send(
                    new GetUsersInSameWorldQuery(userId), cancellationToken);
        var groupName = $"world_{worldName}";
        foreach (var userIdentityId in worldUserIdentities)
        {
            var connectionId = _connectionService.GetConnectionId(userIdentityId);
            if (connectionId is not null)
            {
                await Groups.AddToGroupAsync(connectionId, groupName, cancellationToken);
            }
        }

        return groupName;
    }
}