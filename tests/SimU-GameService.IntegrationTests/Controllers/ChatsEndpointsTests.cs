using SimU_GameService.IntegrationTests.TestUtils;

namespace SimU_GameService.IntegrationTests.Controllers;

public class ChatsEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;

    public ChatsEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public void GetChat_WhenChatExists_ShouldReturnOk()
    {
    }

    [Fact]
    public void DeleteChat_WhenChatExists_ShouldReturnNoContent()
    {
    }

    [Fact]
    public void GetUserChats_WhenUserExists_ShouldReturnOk()
    {
    }

    [Fact]
    public void GetChatHistory_WhenParticipantsExist_ShouldReturnOk()
    {
    }

    [Fact]
    public void AskForQuestion_WhenParticipantsExist_ShouldReturnOk()
    {
    }
}