using System.Net.Http.Json;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text;

namespace SimU_GameService.Infrastructure.Characters;

public class ConversationStatusService : IHostedService, IConversationStatusService
{
    private Timer? _timer;
    private readonly IConversationRepository _conversationRepository;

    public ConversationStatusService(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckConversations, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        return Task.CompletedTask;
    }

    public async void CheckConversations(object? state)
    {
        var activeConversations = await _conversationRepository.GetActiveConversations();

        foreach (var conversation in activeConversations)
        {
            if (DateTime.UtcNow - conversation.LastMessageSentAt > TimeSpan.FromMinutes(15))
            {
                await _conversationRepository.MarkConversationAsOver(conversation.Id);
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