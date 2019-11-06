using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatApp.Match.Model;
using ChatApp.Shared.MessagePackObjects;
using ChatApp.Shared.Services;
using MagicOnion;
using MagicOnion.Server;
using MessagePack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChatApp.Match
{
    public class MatchService : ServiceBase<IMatchService>, IMatchService
    {
        readonly string _fleetName;
        readonly ILogger<MatchService> _logger;
        public MatchService(IConfiguration config, ILogger<MatchService> logger)
        {
            _fleetName = config.GetValue<string>("FLEET_NAME", "default");
            _logger = logger;
        }

        public async UnaryResult<MatchDataReponse> GetAsync(string clientId)
        {
            _logger.LogInformation($"Get GameServer. clientId: {clientId}, fleetName: {_fleetName}");
            var match = await MatchModel.GetOrJoin(clientId, _fleetName);
            _logger.LogInformation($"GameServer Result. clientId: {clientId}, fleetName: {_fleetName}, matchId: {match.MatchId}, Host: {match.RoomData.Host}, Port: {match.RoomData.Port}, connectionCount: {string.Join(',', match.RoomData.ConnectionCount)}");
            return await UnaryResult(match.ToMessagePackObject());
        }

        public async UnaryResult<MatchDataReponse> JoinAsync(string matchId, string clientId)
        {
            _logger.LogInformation($"Join GameServer. matchId: {matchId}, clientId: {clientId}, fleetName: {_fleetName}");
            var match = await MatchModel.Join(matchId, clientId, _fleetName);
            return await UnaryResult(match.ToMessagePackObject());
        }

        public UnaryResult<Nil> LeaveAsync(string matchId, string clientId)
        {
            _logger.LogInformation($"Leave GameServer. matchId: {matchId}, clientId: {clientId}");
            MatchModel.Leave(matchId, clientId);
            return ReturnNil();
        }
    }
}
