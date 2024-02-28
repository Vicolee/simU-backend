using System.Net;
using System.Net.Http.Json;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.IntegrationTests.TestUtils;

public static class TestWorldUtils
{
    internal static async Task<WorldResponse?> CreateWorld(
        HttpClient client,
        Guid creatorId,
        string worldName = Constants.World.TestWorldName,
        string worldDescription = Constants.World.TestWorldDescription)
    {
        var request = new CreateWorldRequest(creatorId, worldName, worldDescription);
        var result = await client.PostAsJsonAsync(
            Constants.Routes.WorldsEndpoints.BaseUri, request);

        return result.StatusCode == HttpStatusCode.OK
            ? await result.Content.ReadFromJsonAsync<WorldResponse>()
            : null;
    }

    internal static async Task<WorldResponse?> GetWorld(
        HttpClient client,
        Guid worldId)
    {
        var result = await client.GetAsync(
            $"{Constants.Routes.WorldsEndpoints.BaseUri}/{worldId}");
        
        return result.StatusCode == HttpStatusCode.OK
            ? await result.Content.ReadFromJsonAsync<WorldResponse>()
            : null;
    }

    internal static async Task<IEnumerable<WorldUserResponse?>?> GetWorldUsers(
        HttpClient client,
        Guid worldId)
    {
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{worldId}/users";
        var result = await client.GetAsync(route);
        
        return result.StatusCode == HttpStatusCode.OK
            ? await result.Content.ReadFromJsonAsync<IEnumerable<WorldUserResponse>>()
            : null;
    }

    internal static async Task<IEnumerable<WorldAgentResponse?>?> GetWorldAgents(
        HttpClient client,
        Guid worldId)
    {
        var route = $"{Constants.Routes.WorldsEndpoints.BaseUri}/{worldId}/agents";
        var result = await client.GetAsync(route);
        
        return result.StatusCode == HttpStatusCode.OK
            ? await result.Content.ReadFromJsonAsync<IEnumerable<WorldAgentResponse>>()
            : null;
    }
}