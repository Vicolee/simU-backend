using SimU_GameService.Infrastructure.Persistence;

namespace SimU_GameService.IntegrationTests.TestUtils; 

public static class TestDatabaseUtils
{
    public static void ClearDatabase(SimUDbContext dbContext)
    {
        using var context = dbContext;
        context.RemoveRange(context.Users);
    }
}