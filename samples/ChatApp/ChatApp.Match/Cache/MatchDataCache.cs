using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatApp.Match.Data;
using ChatApp.Match.KubernetesService;

namespace ChatApp.Match.Cache
{
    public class MatchDataCache
    {
        public static ConcurrentDictionary<string, RoomData> Matchings = new ConcurrentDictionary<string, RoomData>();
        private static ReaderWriterLockSlim lockObj = new ReaderWriterLockSlim();

        public static (string matchId, RoomData room) Create(int connectionLimit, string matchId = "")
        {
            if (string.IsNullOrWhiteSpace(matchId))
            {
                matchId = Guid.NewGuid().ToString();
            }
            // TODO: Retrieve GameServer host:port from AgoneSDK.
            var host = "127.0.0.1";
            var port = 12345;
            var gameServers = GameServer.Current;
            var data = new RoomData
            {
                Id = (host + port).GetHashCode().ToString(),
                Host = host,
                Port = port,
                ConnectionLimit = connectionLimit,
                CreateAt = DateTimeOffset.UtcNow,
                JoinedConnections = new BlockingCollection<ConnectionData>(),
            };

            using (var wl = new WriteLock())
            {
                if (Matchings.TryAdd(matchId, data))
                    throw new KeyNotFoundException("failed to obtain server.");
                return (matchId, data);
            }
        }

        public static (RoomData joinedRoom, bool isCompleted) Join(string matchId, ConnectionData connection)
        {
            var room = Get(matchId);
            using (var wl = new WriteLock(lockObj))
            {
                if (!room.JoinedConnections.TryAdd(connection))
                    throw new Exception($"failed to join match: {matchId}");
                return (room, room.JoinedConnections.Count == room.JoinedConnections.BoundedCapacity);
            }
        }

        public static bool Exists(string matchId)
        {
            return Matchings.ContainsKey(matchId);
        }

        public static (bool exists, string matchId) Exists(ConnectionData connection)
        {
            foreach (var match in Matchings)
            {
                if (match.Value.JoinedConnections.Any(x => x.ClientId == connection.ClientId))
                {
                    return (true, match.Key);
                }
            }
            return (false, "");
        }

        public static RoomData Get(string matchId)
        {
            if (!Matchings.TryGetValue(matchId, out RoomData data))
                throw new ArgumentException($"could not find matchId:{matchId}");
            return data;
        }

        public static void Delete(string matchId)
        {
            using (var wl = new WriteLock(lockObj))
            {
                if (!Matchings.TryRemove(matchId, out var value))
                {
                    throw new Exception($"Could not remove matchId: matchId");
                }
            }
        }

        public static RoomData Leave(string matchId, string clientId)
        {
            var connection = new ConnectionData
            {
                ClientId = clientId,
            };
            var room = Get(matchId);
            using (var wl = new WriteLock(lockObj))
            {
                if (!room.JoinedConnections.Remove<ConnectionData>(connection))
                    throw new Exception($"failed to leave from match: {matchId}");
                if (room.JoinedConnections.Count == 0)
                {
                    Delete(matchId);
                }
                return room;
            }
        }
    }

    public static class BlockingCollectionExtensions
    {
        public static bool Remove<T>(this BlockingCollection<T> self, T itemToRemove)
        {
            lock (self)
            {
                T comparedItem;
                var itemsList = new List<T>();
                do
                {
                    var result = self.TryTake(out comparedItem);
                    if (!result)
                        return false;
                    if (!comparedItem.Equals(itemToRemove))
                    {
                        itemsList.Add(comparedItem);
                    }
                }
                while (!(comparedItem.Equals(itemToRemove)));
                Parallel.ForEach(itemsList, t => self.Add(t));
            }

            return true;
        }
    }
}
