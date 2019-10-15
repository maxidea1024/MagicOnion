using ChatApp.Shared.Services;
using MagicOnion;
using MagicOnion.Agones;
using MagicOnion.Server;
using MessagePack;

namespace ChatApp.Server
{
    public class AgonesService : ServiceBase<IAgonesService>, IAgonesService
    {
        readonly IAgonesSdk _agones;
        public AgonesService(IAgonesSdk agones)
        {
            _agones = agones;
        }
        public async UnaryResult<bool> Allocate()
        {
            var res = await _agones.Allocate();
            return res;
        }

        public async UnaryResult<(string, int)> GetGameServer()
        {
            var res = await _agones.GameServer();
            if (res.ok)
            {
                return (res.response.status.address, res.response.status.ports[0].port);
            }
            else
            {
                return ("", 0);
            }
        }
    }
}
