using Microsoft.AspNetCore.SignalR;

namespace SimU_GameService.Api.Hubs;

public class UnityClientHub : Hub<IUnityClient>
{
    /// <inheritdoc/>
    public override async Task OnConnectedAsync()
    {
        // TODO: Add functionality later. For now, just call the base method.
        await base.OnConnectedAsync();
    }

    /// <inheritdoc/>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // TODO: Add functionality later. For now, just call the base method.
        await base.OnDisconnectedAsync(exception);
    }
}
