using Microsoft.AspNetCore.SignalR;

namespace SimU_GameService.Api.Filters;

public class ErrorHandlingHubFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch (Exception ex)
        {
            await invocationContext.Hub.Clients.Caller.SendAsync("HandleError", ex.Message);
            return null;
        }
    }
}