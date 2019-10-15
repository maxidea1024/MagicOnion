using System;
using System.Collections.Concurrent;
using MessagePack;

namespace ChatApp.Shared.MessagePackObjects
{
    [MessagePackObject]
    public struct MatchDataReponse
    {
        [Key(0)]
        public string MatchId { get; set; }

        [Key(1)]
        public string ClientId { get; set; }

        [Key(2)]
        public RoomDataResponse Room { get; set; }
    }

    [MessagePackObject]
    public struct RoomDataResponse
    {
        [Key(0)]
        public string Id { get; set; }
        [Key(1)]
        public string Host { get; set; }
        [Key(2)]
        public int Port { get; set; }
        [Key(3)]
        public int ConnectionNumber { get; set; }
        [Key(4)]
        public int ConnectionLimit { get; set; }
        [Key(5)]
        public DateTimeOffset CreateAt { get; set; }
        [Key(6)]
        public string[] JoinedConnections { get; set; }
    }
}
