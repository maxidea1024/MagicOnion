using System;
using System.Collections.Generic;
using System.Text;
using ChatApp.Match.Model;
using ChatApp.Shared.MessagePackObjects;
using ChatApp.Shared.Services;
using MagicOnion;
using MagicOnion.Server;
using MessagePack;
using Microsoft.Extensions.Logging;

namespace ChatApp.Match
{
    public class MatchService : ServiceBase<IMatchService>, IMatchService
    {
        readonly ILogger<MatchService> _logger;
        public MatchService(ILogger<MatchService> logger)
        {
            _logger = logger;
        }

        public UnaryResult<MatchDataReponse> GetAsync(string clientId)
        {
            _logger.LogInformation($"Get clientId. {clientId}");
            var match = MatchModel.GetOrJoin(clientId);
            return UnaryResult(match.ToMessagePackObject());
        }

        public UnaryResult<MatchDataReponse> JoinAsync(string matchId, string clientId)
        {
            _logger.LogInformation($"Join matching id. {matchId}");
            var match = MatchModel.Join(matchId, clientId);
            return UnaryResult(match.ToMessagePackObject());
        }

        public UnaryResult<Nil> LeaveAsync(string matchId, string clientId)
        {
            _logger.LogInformation($"Leave matching id. {matchId}");
            MatchModel.Leave(matchId, clientId);
            return ReturnNil();
        }
    }
}
