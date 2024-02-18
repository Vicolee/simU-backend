using System.Net.Http.Json;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text;

namespace SimU_GameService.Infrastructure.Characters;

public class OnlineStatusService : IHostedService, IOnlineStatusService
{
    private readonly Dictionary<Guid, bool> _userResponses;
    private Timer? _timer;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly HttpClient _httpClient;

    public OnlineStatusService(IUserRepository userRepository, IChatRepository chatRepository, HttpClient httpClient)
    {
        _userResponses = new Dictionary<Guid, bool>();
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _httpClient = httpClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
       _timer = new Timer(state => CheckOnlineStatus(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
       return Task.CompletedTask;
    }

    public async void CheckOnlineStatus()
    {
        var onlineUsers = await _userRepository.GetOnlineUsers();

        // Sends a message to each online user, and waits for a response
        foreach (var userId in onlineUsers)
        {
            _userResponses[userId] = false;
            SendMessageToUser(userId);
        }

        // Waits 3 minutes for responses
        await Task.Delay(TimeSpan.FromMinutes(3)).ContinueWith(t => MarkOfflineUsers());
    }

    private void SendMessageToUser(Guid userId)
    {
        var message = new { Text = $"Online Status Check: Respond 'ONLINE: {userId}' if you're still active in this world." };
        var request = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

        _httpClient.PostAsJsonAsync("", request);
    }

    public void SetOnlineStatus(Guid userId)
    {
        if (_userResponses.ContainsKey(userId))
        {
            _userResponses[userId] = true;
        }
    }

    private async void MarkOfflineUsers()
    {
        // Mark users who haven't responded as offline
        foreach (var userResponse in _userResponses)
        {
            if (!userResponse.Value)
            {
                await _userRepository.MarkOffline(userResponse.Key);
            }
        }

        // Clear the responses for the next check
        _userResponses.Clear();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Stop the timer and dispose of it
        _timer?.Change(Timeout.Infinite, 0);
        _timer?.Dispose();
        _timer = null;

        return Task.CompletedTask;
    }
}