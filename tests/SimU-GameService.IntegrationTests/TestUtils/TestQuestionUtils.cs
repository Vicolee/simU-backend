using System.Net;
using System.Net.Http.Json;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;

namespace SimU_GameService.IntegrationTests.TestUtils;

public static class TestQuestionUtils
{
    internal static async Task PostQuestion(
        HttpClient client,
        string questionText,
        string type)
    {
        var request = new PostQuestionRequest(questionText, type);
        var result = await client.PostAsJsonAsync(
            Constants.Routes.QuestionsEndpoints.BaseUri, request);

        if (result.StatusCode != HttpStatusCode.NoContent)
        {
            throw new Exception("Failed to post question");
        }
    }

    internal static async Task<IEnumerable<QuestionResponse>?> GetUserQuestions(
        HttpClient client)
    {
        var result = await client.GetAsync(Constants.Routes.QuestionsEndpoints.GetUserQuestions);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Failed to get user questions");
        }

        return await result.Content.ReadFromJsonAsync<IEnumerable<QuestionResponse>>();
    }

    internal static async Task<IEnumerable<QuestionResponse>?> GetAgentQuestions(
        HttpClient client)
    {
        var result = await client.GetAsync(Constants.Routes.QuestionsEndpoints.GetAgentQuestions);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Failed to get agent questions");
        }

        return await result.Content.ReadFromJsonAsync<IEnumerable<QuestionResponse>>();
    }

    internal static async Task<SummaryResponse?> PostResponses(
        HttpClient client,
        ResponsesRequest request)
    {
        var result = await client.PostAsJsonAsync(
            Constants.Routes.QuestionsEndpoints.PostResponses, request);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Failed to post responses");
        }

        return await result.Content.ReadFromJsonAsync<SummaryResponse>();
    }
}