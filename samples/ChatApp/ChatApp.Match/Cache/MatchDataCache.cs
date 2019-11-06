using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgonesPod;
using ChatApp.Match.Data;

namespace ChatApp.Match.Cache
{
    public class MatchDataCache
    {
        // Create / Leave のタイミングで更新
        // GameServerName : connections
        public static ConcurrentDictionary<string, int> GameServerConnectionStatus = new ConcurrentDictionary<string, int>();
        // MatchId : RoomData
        public static ConcurrentDictionary<string, RoomData> Matchings = new ConcurrentDictionary<string, RoomData>();
        private static ReaderWriterLockSlim lockObj = new ReaderWriterLockSlim();

        public static async ValueTask<(string matchId, RoomData room)> Create(int connectionLimit, string fleetName, string matchId = "")
        {
            if (string.IsNullOrWhiteSpace(matchId))
            {
                matchId = Guid.NewGuid().ToString();
            }

            // GameServer の状態を更新 (いつの間にか死んでたとか)
            UpdateGameServerConnectionStatus();

            // connection limit までは新規Allocate せずに既存のGameServer を使う
            // TODO: Save state to RedisBack Plane for reboot.
            var gameServer = GameServerConnectionStatus.Where(x => x.Value < connectionLimit).FirstOrDefault();
            IGameServerInfo gameserverInfo = gameServer.Key == null 
                ? await GameServer.AllocateAsync(fleetName)
                : GameServer.Current.Where(x => x.Name == gameServer.Key).FirstOrDefault();

            if (!gameserverInfo.IsAllocated)
                throw new Exception("Agones could not allocate new node for request.");

            var name = gameserverInfo.Name;
            var host = gameserverInfo.Address;
            var port = gameserverInfo.Port;
            var data = new RoomData
            {
                Id = (host + port).GetHashCode().ToString(),
                Host = host,
                Port = port,
                ConnectionLimit = connectionLimit,
                CreateAt = DateTimeOffset.UtcNow,
                JoinedConnections = new ConcurrentDictionary<string, ConnectionData>(),
            };

            using (var wl = new WriteLock(lockObj))
            {
                if (!Matchings.TryAdd(matchId, data))
                    throw new KeyNotFoundException("failed to obtain server.");
                return (matchId, data);
            }

            void UpdateGameServerConnectionStatus()
            {
                var current = AgonesPod.GameServer.Current;
                foreach (var item in current)
                {
                    if (!GameServerConnectionStatus.TryGetValue(item.Name, out var _))
                    {
                        GameServerConnectionStatus.TryAdd(item.Name, 0);
                    }
                }
            }
        }

        public static void Join(string matchId, ConnectionData connection, RoomData room)
        {
            using (var wl = new WriteLock(lockObj))
            {
                if (room.JoinedConnections.TryAdd(connection.ClientId, connection))
                {
                    room.ConnectionCount++;
                }
                else
                {
                    throw new Exception($"failed to join match: {matchId}");
                }
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
                if (match.Value.JoinedConnections.Any(x => x.Key == connection.ClientId))
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
            // Agones Serverは殺さない
            // TODO: Agones Server を外からShutdown したくない。ので殺すときはアプリケーションから agones sdk の Shutdown を呼び出したいお気持ち
            var room = Get(matchId);
            using (var wl = new WriteLock(lockObj))
            {
                if (room.JoinedConnections.TryRemove(clientId, out var _))
                {
                    room.ConnectionCount--;
                }
                else
                {
                    throw new Exception($"failed to leave from match: {matchId}");
                }
            }

            // 最後の接続者だったときにMatching が消す
            if (room.JoinedConnections.Count == 0)
            {
                Delete(matchId);
            }
            return room;
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
