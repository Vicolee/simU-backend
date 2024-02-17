using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class CachedLocationRepository : ILocationRepository
{
    private readonly ILocationRepository _decorated;
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, Location> _laskKnownLocations;
    private readonly Timer _timer;

    public CachedLocationRepository(ILocationRepository decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
        _laskKnownLocations = new();
        _timer = new Timer(PersistLastKnownLocations, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    public Task<Location?> GetLocation(Guid characterId, bool isAgent = false)
    {
        string key = isAgent ? $"Agent_{characterId}" : $"User_{characterId}";
        return _cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
            return _decorated.GetLocation(characterId, isAgent);
        });
    }

    public Task UpdateLocation(Guid characterId, int x_coord, int y_coord, bool isAgent = false)
    {
        string key = isAgent ? $"Agent_{characterId}" : $"User_{characterId}";
        var location = new Location(x_coord, y_coord);

        _cache.Set(key, location, TimeSpan.FromMinutes(2));
        _laskKnownLocations.TryAdd(key, location);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Timer callback to persist last known locations to the database.
    /// </summary>
    /// <param name="state">Default is null</param>
    private async void PersistLastKnownLocations(object? state)
    {
        foreach (var (key, location) in _laskKnownLocations)
        {
            Guid characterId = Guid.Parse(key.Split('_')[1]);
            bool isAgent = key.StartsWith("Agent");

            await _decorated.UpdateLocation(characterId, location.X_coord, location.Y_coord, isAgent);
            _laskKnownLocations.TryRemove(key, out _);
        }
    }
}