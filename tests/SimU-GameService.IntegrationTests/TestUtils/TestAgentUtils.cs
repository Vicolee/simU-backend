using System.Net;
using System.Net.Http.Json;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.IntegrationTests.TestUtils;

public static class TestAgentUtils
{
    internal static async Task<AgentResponse?> CreateAgent(HttpClient client, Guid creatorId,
        string agentName = Constants.Agent.TestAgentName,
        string agentDescription = Constants.Agent.TestAgentDescription,
        float incubationPeriodInHours = Constants.Agent.TestMaxIncubationPeriodInHours,
        string spriteUri = Constants.Agent.TestSpriteUri,
        string spriteHeadshotUri = Constants.Agent.TestSpriteHeadshotUri)
    {
        var request = new CreateAgentRequest(agentName,
            agentDescription,
            creatorId,
            incubationPeriodInHours,
            new Uri(spriteUri),
            new Uri(spriteHeadshotUri));

        var result = await client.PostAsJsonAsync(
            Constants.Routes.AgentsEndpoints.BaseUri, request);

        return result.StatusCode == HttpStatusCode.OK
            ? await result.Content.ReadFromJsonAsync<AgentResponse>()
            : null;
    }

    internal static async Task<AgentResponse?> GetAgent(
        HttpClient client,
        Guid agentId)
    {
        var result = await client.GetAsync(
            $"{Constants.Routes.AgentsEndpoints.BaseUri}/{agentId}");
        
        return result.StatusCode == HttpStatusCode.OK
            ? await result.Content.ReadFromJsonAsync<AgentResponse>()
            : null;
    }
}