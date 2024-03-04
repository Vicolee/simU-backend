using SimU_GameService.Application.Abstractions.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SimU_GameService.Infrastructure.Characters;

public class ConversationStatusService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(15);

    public ConversationStatusService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var conversationRepository = scope.ServiceProvider.GetRequiredService<IConversationRepository>();
            var activeConversations = await conversationRepository.GetActiveConversations();

            foreach (var conversation in activeConversations)
            {
                if (DateTime.UtcNow - conversation.LastMessageSentAt > _checkInterval)
                {
                    await conversationRepository.MarkConversationAsOver(conversation.Id);
                }
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}