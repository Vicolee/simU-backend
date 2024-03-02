using System.Net;
using System.Net.Http.Json;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.IntegrationTests.TestUtils;

namespace SimU_GameService.IntegrationTests.Controllers;

public class QuestionsEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;

    public QuestionsEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async void PostQuestion_WhenValid_ShouldReturnNoContent()
    {
        // arrange
        var request = new PostQuestionRequest("Test user question", "UserQuestion");

        // act
        var result = await _client.PostAsJsonAsync(Constants.Routes.QuestionsEndpoints.BaseUri, request);

        // assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
    }

    [Fact]
    public async void GetUserQuestions_ShouldReturnOk()
    {
        // arrange
        // post questions
        await TestQuestionUtils.PostQuestion(_client, "Test user question", "UserQuestion");
        await TestQuestionUtils.PostQuestion(_client, "Test agent question", "AgentQuestion");
        await TestQuestionUtils.PostQuestion(_client, "Test user or agent question", "UserOrAgentQuestion");

        // act
        var result = await _client.GetAsync(Constants.Routes.QuestionsEndpoints.GetUserQuestions);
        var response = await result.Content.ReadFromJsonAsync<IEnumerable<QuestionResponse>>();

        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.NotEmpty(response);
        Assert.Contains("Test user", response.ElementAt(0).Question);
        Assert.Contains("Test user", response.ElementAt(1).Question);
    }

    [Fact]
    public async Task GetAgentQuestions_ShouldReturnOkAsync()
    {
        // arrange
        // post questions
        await TestQuestionUtils.PostQuestion(_client, "Test user question", "UserQuestion");
        await TestQuestionUtils.PostQuestion(_client, "Test agent question", "AgentQuestion");
        await TestQuestionUtils.PostQuestion(_client, "Test user or agent question", "UserOrAgentQuestion");

        // act
        var result = await _client.GetAsync(Constants.Routes.QuestionsEndpoints.GetAgentQuestions);
        var response = await result.Content.ReadFromJsonAsync<IEnumerable<QuestionResponse>>();

        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.NotEmpty(response);
        Assert.Contains("agent question", response.ElementAt(0).Question);
        Assert.Contains("agent question", response.ElementAt(1).Question);
    }

    [Fact]
    public async Task PostResponses_WhenValid_ShouldReturnOkAsync()
    {
        // arrange
        // post questions
        await TestQuestionUtils.PostQuestion(_client, "Test user question", "UserQuestion");
        await TestQuestionUtils.PostQuestion(_client, "Test agent question", "AgentQuestion");
        await TestQuestionUtils.PostQuestion(_client, "Test user or agent question", "UserOrAgentQuestion");

        // get user questions to get question ids
        var userQuestionIds = await TestQuestionUtils
            .GetUserQuestions(_client)
            .ContinueWith(t => t.Result?.Select(q => q.Id).ToList());

        var agentQuestionIds = await TestQuestionUtils
            .GetAgentQuestions(_client)
            .ContinueWith(t => t.Result?.Select(q => q.Id).ToList());

        // register user and agent
        var user = await TestUserUtils.RegisterUser(_client, Constants.User.TestEmail);
        var agent = await TestAgentUtils.CreateAgent(_client, user!.Id);

        // generate responses request for user
        var userResponses = userQuestionIds!
            .Select(id => new IdResponsePair(id, "Test user response"))
            .ToList();
        var userResponsesRequest = new ResponsesRequest(user!.Id, user.Id, userResponses);

        // generate responses request for agent
        var agentResponses = agentQuestionIds!
            .Select(id => new IdResponsePair(id, "Test agent response"))
            .ToList();
        var agentResponsesRequest = new ResponsesRequest(user!.Id, agent!.Id, agentResponses);

        // act
        var userResult = await TestQuestionUtils.PostResponses(_client, userResponsesRequest);
        var agentResult = await TestQuestionUtils.PostResponses(_client, agentResponsesRequest);

        // assert
        Assert.NotNull(userResult);
        Assert.NotNull(agentResult);
    }

    [Fact]
    public void PostResponse_WhenValid_ShouldReturnOk()
    {
    }

    [Fact]
    public void GetResponses_WhenTargetExists_ShouldReturnOk()
    {
    }

    [Fact]
    public void GetQuestionResponse_WhenTargetAndQuestionExists_ShouldReturnOk()
    {
    }
}
