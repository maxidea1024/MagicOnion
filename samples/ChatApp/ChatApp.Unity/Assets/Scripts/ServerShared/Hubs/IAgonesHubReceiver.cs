using ChatApp.Shared.MessagePackObjects;

namespace ChatApp.Shared.Hubs
{
    /// <summary>
    /// Server -> Client API
    /// </summary>
    public interface IAgonesHubReceiver
    {
        void OnGetGameServer(string endpoint);
        void OnExitGameServer();
    }
}
