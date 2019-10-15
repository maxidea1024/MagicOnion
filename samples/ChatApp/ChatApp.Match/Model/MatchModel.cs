using System;
using System.Collections.Generic;
using System.Text;
using ChatApp.Match.Cache;
using ChatApp.Match.Data;
using ChatApp.Shared.MessagePackObjects;

namespace ChatApp.Match.Model
{
    public class MatchModel
    {
        public string MatchId { get; private set; }
        public RoomData RoomData { get; private set; }
        public ConnectionData ConnectionData { get; private set; }

        private MatchModel(string clientId)
        {
            this.ConnectionData = new ConnectionData
            {
                ClientId = clientId,
            };
        }
        private void GetOrJoin()
        {
            var (exists, matchId) = MatchDataCache.Exists(this.ConnectionData);
            if (exists)
            {
                this.RoomData = MatchDataCache.Get(matchId);
                this.MatchId = matchId;
            }
            else
            {
                (this.MatchId, this.RoomData) = MatchDataCache.Create(1000);
                MatchDataCache.Join(this.MatchId, ConnectionData);
            }
        }
        private void Join(string matchId)
        {
            if (!MatchDataCache.Exists(matchId))
                throw new ArgumentOutOfRangeException($"matchId not found from existing: {matchId}");
            this.RoomData = MatchDataCache.Get(matchId);
            this.MatchId = matchId;
        }

        public static MatchModel GetOrJoin(string clientId)
        {
            var model = new MatchModel(clientId);
            model.GetOrJoin();
            return model;
        }

        public static MatchModel Join(string matchId, string clientId)
        {
            var model = new MatchModel(clientId);
            model.Join(matchId);
            return model;
        }

        public static void Leave(string matchId, string clientId)
        {
            MatchDataCache.Leave(matchId, clientId);
        }

        public static void Delete(string matchId)
        {
            MatchDataCache.Delete(matchId);
        }

        public MatchDataReponse ToMessagePackObject()
        {
            return new MatchDataReponse
            {
                MatchId = this.MatchId,
                ClientId = this.ConnectionData.ClientId,
                Room = this.RoomData.ToMessagePackObject(),
                
            };
        }
    }
}
