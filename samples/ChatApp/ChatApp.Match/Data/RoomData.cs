using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.Shared.MessagePackObjects;

namespace ChatApp.Match.Data
{
    public class RoomData
    {
        public string Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int ConnectionCount { get; set; }
        public int ConnectionLimit { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public ConcurrentDictionary<string, ConnectionData> JoinedConnections { get; set; }
    }

    public static class RoomDataExtensions
    {
        /// <summary>
        /// DataクラスからMessagePackObjectに変換
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static RoomDataResponse ToMessagePackObject(this RoomData self)
        {
            return new RoomDataResponse
            {
                Id = self.Id,
                Host = self.Host,
                Port = self.Port,
                ConnectionNumber = self.ConnectionCount,
                ConnectionLimit= self.ConnectionLimit,
                CreateAt = self.CreateAt,
                JoinedConnections = self.JoinedConnections.Select(x => x.Key).ToArray(),
            };
        }
    }
}
