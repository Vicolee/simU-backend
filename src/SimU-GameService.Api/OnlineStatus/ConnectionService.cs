using System.Collections.Concurrent;

namespace SimU_GameService.Api.OnlineStatus;

public class ConnectionService
{
    private readonly ConcurrentDictionary<string, string> _connectionIdMap = new();
    private readonly ConcurrentDictionary<string, DateTime> _lastPingMap = new();

    public void AddConnection(string identityId, string connectionId)
        => _connectionIdMap[identityId] = connectionId;

    public void RemoveConnectionByIdentityId(string identityId)
    {
        var item = _connectionIdMap.FirstOrDefault(x => x.Key == identityId);
        if (!item.Equals(default(KeyValuePair<string, string>)))
        {
            _connectionIdMap.TryRemove(item.Key, out _);
        }
    }

    public void RemoveConnectionByConnectionId(string connectionId)
    {
        var item = _connectionIdMap.FirstOrDefault(x => x.Value == connectionId);
        if (!item.Equals(default(KeyValuePair<string, string>)))
        {
            _connectionIdMap.TryRemove(item.Key, out _);
        }
    }

    public string? GetConnectionId(string identityId)
    {
        _connectionIdMap.TryGetValue(identityId, out var connectionId);
        return connectionId;
    }

    public string? GetIdentityIdFromConnectionId(string connectionId)
        => _connectionIdMap.FirstOrDefault(x => x.Value == connectionId).Key;

    public IEnumerable<string> GetAllConnectionIds() => _connectionIdMap.Values;

    public void UpdateLastPingTime(string identityId)
        => _lastPingMap[identityId] = DateTime.UtcNow;

    public void RemoveLastPingTime(string identityId) 
        => _lastPingMap.TryRemove(identityId, out _);

    public IEnumerable<KeyValuePair<string, DateTime>> GetAllLastPingTimes() => _lastPingMap;
}