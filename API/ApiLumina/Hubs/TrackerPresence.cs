using System.Collections.Concurrent;

namespace ApiLumina.Hubs;
    public class PresenceTracker
    {
        private readonly ConcurrentDictionary<string, List<string>> _onlineUsers = new();

        public Task<bool> UserConnected(string userId, string connectionId)
        {
            bool isFirstConnection = false;
            _onlineUsers.AddOrUpdate(userId,
                new List<string> { connectionId },
                (key, list) => {
                    lock (list) { list.Add(connectionId); }
                    return list;
                });

            if (_onlineUsers[userId].Count == 1) isFirstConnection = true;

            return Task.FromResult(isFirstConnection);
        }

        public Task<bool> UserDisconnected(string userId, string connectionId)
        {
            bool isLastConnection = false;
            if (_onlineUsers.TryGetValue(userId, out var connections))
            {
                lock (connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _onlineUsers.TryRemove(userId, out _);
                        isLastConnection = true;
                    }
                }
            }
            return Task.FromResult(isLastConnection);
        }

        public Task<string[]> GetOnlineUsers()
        {
            return Task.FromResult(_onlineUsers.Keys.ToArray());
        }
    }

