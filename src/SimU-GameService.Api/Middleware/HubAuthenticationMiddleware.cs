namespace SimU_GameService.Api.Middleware;

public class HubAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public HubAuthenticationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        bool isUnityHub = request.Path.StartsWithSegments("/unity", StringComparison.OrdinalIgnoreCase);
        bool hasAccessToken = request.Query.TryGetValue("access_token", out var accessToken);

        if (isUnityHub && hasAccessToken)
        {
            request.Headers.Add("Authorization", $"Bearer {accessToken}");
        }

        await _next(context);
    }
}