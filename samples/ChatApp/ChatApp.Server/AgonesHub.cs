using ChatApp.Shared.Agones;
using ChatApp.Shared.Hubs;
using ChatApp.Shared.MessagePackObjects;
using MagicOnion.Agones;
using MagicOnion.Server.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Server
{
    /// <summary>
    /// Agones server processing.
    /// One class instance for one connection.
    /// </summary>
    public class AgonesHub : StreamingHubBase<IAgonesHub, IAgonesHubReceiver>, IAgonesHub
    {
        readonly IAgonesSdk _agones;
        public AgonesHub(IAgonesSdk agones)
        {
            _agones = agones;
        }

        public async Task<bool> AllocateAsync()
        {
            var res = await _agones.Allocate();
            return res;
        }

        public async Task<GameServerResponse> GetGameServerAsync()
        {
            var res = await _agones.GameServer();
            if (res.ok)
            {
                return res.response;
            }
            return null;
        }

        public async Task ExitGameServerAsync()
        {
            await _agones.Shutdown();
        }
    }
}
