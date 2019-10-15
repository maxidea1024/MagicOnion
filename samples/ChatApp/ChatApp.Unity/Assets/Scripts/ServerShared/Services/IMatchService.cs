using ChatApp.Shared.MessagePackObjects;
using MagicOnion;
using MessagePack;

namespace ChatApp.Shared.Services
{
    /// <summary>
    /// Client -> Match API
    /// </summary>
    public interface IMatchService : IService<IMatchService>
    {
        /// <summary>
        /// join a matching room
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        UnaryResult<MatchDataReponse> JoinAsync(string matchId, string clientId);
        /// <summary>
        /// get a matching room
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        UnaryResult<MatchDataReponse> GetAsync(string clientId);
        /// <summary>
        /// left a matching room
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        UnaryResult<Nil> LeaveAsync(string matchId, string clientId);
    }
}
