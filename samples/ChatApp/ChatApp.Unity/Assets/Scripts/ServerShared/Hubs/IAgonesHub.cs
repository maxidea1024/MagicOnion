using ChatApp.Shared.Agones;
using ChatApp.Shared.MessagePackObjects;
using MagicOnion;
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
        Task<bool> AllocateAsync();
        Task<GameServerResponse> GetGameServerAsync();

        Task ExitGameServerAsync();
    }
}
