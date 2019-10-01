using ChatApp.Shared.MessagePackObjects;
using MagicOnion;
using MagicOnion.Agones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Shared.Hubs
{
    /// <summary>
    /// Client -> Server API (Streaming)
    /// </summary>
    public interface IAgonesHub : IStreamingHub<IAgonesHub, IAgonesHubReceiver>
    {
        Task<GameServerResponse> GetGameServerAsync();

        Task ExitGameServerAsync();
    }
}
