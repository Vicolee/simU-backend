using SimU_GameService.IntegrationTests.TestUtils;

namespace SimU_GameService.IntegrationTests.Controllers;

public class AgentsEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;

    public AgentsEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public void CreateAgent_WhenValid_ShouldReturnOk()
    {
    }

    [Fact]
    public void GetAgent_WhenAgentExists_ShouldReturnOk()
    {
    }

    [Fact]
    public void GetAgentSummary_WhenAgentExists_ShouldReturnOk()
    {
    }

    [Fact]
    public void PostVisualDescription_WhenValid_ShouldReturnOk()
    {
    }
}