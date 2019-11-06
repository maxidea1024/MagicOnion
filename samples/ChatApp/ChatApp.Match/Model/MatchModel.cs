using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        private MatchModel(string clientId, string fleetName)
        {
            this.ConnectionData = new ConnectionData
            {
                ClientId = clientId,
                FleetName = fleetName,
            };
        }
        private async Task GetOrJoin()
        {
            // 1. already clientid exists in some matching -> use existing matching & room
            // 2. new client -> create new matching & room
            var (exists, matchId) = MatchDataCache.Exists(this.ConnectionData);
            if (exists)
            {
                // 1.
                this.RoomData = MatchDataCache.Get(matchId);
                this.MatchId = matchId;
            }
            else
            {
                // 2.
                var (createdMatchId, roomData) = await MatchDataCache.Create(1000, this.ConnectionData.FleetName);
                MatchDataCache.Join(createdMatchId, ConnectionData, roomData);
                this.RoomData = roomData;
                this.MatchId = createdMatchId;
            }
        }
        private async Task Join(string matchId)
        {
            if (!MatchDataCache.Exists(matchId))
            {
                //throw new ArgumentOutOfRangeException($"matchId not found from existing: {matchId}");
                var (_, roomData) = await MatchDataCache.Create(1000, this.ConnectionData.FleetName, matchId);
                MatchDataCache.Join(matchId, this.ConnectionData, roomData);
                this.RoomData = roomData;
                this.MatchId = matchId;
            }
            else
            {
                this.RoomData = MatchDataCache.Get(matchId);
                this.MatchId = matchId;
            }
        }

        public static async ValueTask<MatchModel> GetOrJoin(string clientId, string fleetName)
        {
            var model = new MatchModel(clientId, fleetName);
            await model.GetOrJoin();
            return model;
        }

        public static async ValueTask<MatchModel> Join(string matchId, string clientId, string fleetName)
        {
            var model = new MatchModel(clientId, fleetName);
            await model.Join(matchId);
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
