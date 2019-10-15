using ChatApp.Shared.MessagePackObjects;
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

        public async UnaryResult<AgonesGameServerResponse> GetGameServer()
        {
            var res = await _agones.GameServer();
            if (res.ok)
            {
                return new AgonesGameServerResponse
                {
                    Host = res.response.status.address,
                    Port = res.response.status.ports[0].port
                };
            }
            else
            {
                return new AgonesGameServerResponse();
            }
        }
    }
}
