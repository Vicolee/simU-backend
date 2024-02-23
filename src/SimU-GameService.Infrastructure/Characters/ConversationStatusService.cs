using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SimU_GameService.Infrastructure.Characters;

public class ConversationStatusService : IHostedService, IConversationStatusService
{
    private Timer? _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ConversationStatusService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckConversations, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        return Task.CompletedTask;
    }

    public async void CheckConversations(object? state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var conversationRepository = scope.ServiceProvider.GetRequiredService<IConversationRepository>();
            var activeConversations = await conversationRepository.GetActiveConversations();

            foreach (var conversation in activeConversations)
            {
                if (DateTime.UtcNow - conversation.LastMessageSentAt > TimeSpan.FromMinutes(15))
                {
                    await conversationRepository.MarkConversationAsOver(conversation.Id);
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
        _timer = null;

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}