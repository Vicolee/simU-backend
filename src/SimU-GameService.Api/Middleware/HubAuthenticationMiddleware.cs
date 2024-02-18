namespace SimU_GameService.Api.Middleware;

public class HubAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public HubAuthenticationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        if (request.Path.StartsWithSegments("/unity", StringComparison.OrdinalIgnoreCase)
            && request.Query.TryGetValue("access_token", out var accessToken))
        {
            request.Headers.Add("Authorization", $"Bearer {accessToken}");
        }

        await _next(context);
    }
}