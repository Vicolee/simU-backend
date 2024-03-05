using System.Collections.Concurrent;

namespace SimU_GameService.Api.OnlineStatus;

public class ConnectionService
{
    private readonly ConcurrentDictionary<string, string> _connectionIdentityMap = new();
    private readonly ConcurrentDictionary<string, DateTime> _connectionPingMap = new();
    private readonly ConcurrentDictionary<string, Guid> _identityUserIdMap = new();

    public void AddConnection(string identityId, string connectionId)
        => _connectionIdentityMap[identityId] = connectionId;

    public void RemoveConnectionByIdentityId(string identityId)
    {
        var item = _connectionIdentityMap.FirstOrDefault(x => x.Key == identityId);
        if (!item.Equals(default(KeyValuePair<string, string>)))
        {
            _connectionIdentityMap.TryRemove(item.Key, out _);
        }
    }

    public void RemoveConnection(string connectionId)
    {
        var item = _connectionIdentityMap.FirstOrDefault(x => x.Value == connectionId);
        if (!item.Equals(default(KeyValuePair<string, string>)))
        {
            _connectionIdentityMap.TryRemove(item.Key, out _);
        }
    }

    public string? GetConnectionId(string identityId)
    {
        _connectionIdentityMap.TryGetValue(identityId, out var connectionId);
        return connectionId;
    }

    public string? GetIdentityIdFromConnectionId(string connectionId)
        => _connectionIdentityMap.FirstOrDefault(x => x.Value == connectionId).Key;
    
    public DateTime? GetLastPingTime(string identityId)
    {
        _connectionPingMap.TryGetValue(identityId, out var lastPing);
        return lastPing;
    }

    public IEnumerable<string> GetAllConnectionIds() => _connectionIdentityMap.Values;

    public void UpdateLastPingTime(string identityId)
        => _connectionPingMap[identityId] = DateTime.UtcNow;

    public void RemoveLastPingTime(string identityId) 
        => _connectionPingMap.TryRemove(identityId, out _);

    public IEnumerable<KeyValuePair<string, DateTime>> GetAllLastPingTimes() => _connectionPingMap;

    public Guid? GetUserIdFromIdentityId(string identityId)
    {
        _identityUserIdMap.TryGetValue(identityId, out var userId);
        return userId;
    }

    public void AddIdentityUserIdMapping(string identityId, Guid userId)
    {
        _identityUserIdMap[identityId] = userId;
    }

    public void RemoveIdentityUserIdMapping(string connectionId)
    {
        var identityId = GetIdentityIdFromConnectionId(connectionId);
        if (identityId is not null)
        {
            _identityUserIdMap.TryRemove(identityId, out _);
        }
    }

    public void UpdateUserIdentityMap(IEnumerable<(Guid, string)> userIdentityPairs)
    {
        foreach (var (userId, identityId) in userIdentityPairs)
        {
            _identityUserIdMap[identityId] = userId;
        }
    }
}