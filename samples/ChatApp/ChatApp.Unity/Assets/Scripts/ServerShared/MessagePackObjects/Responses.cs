using MessagePack;

namespace ChatApp.Shared.MessagePackObjects
{
    /// <summary>
    /// Message information
    /// </summary>
    [MessagePackObject]
    public struct MessageResponse
    {
        [Key(0)]
        public string UserName { get; set; }

        [Key(1)]
        public string Message { get; set; }
    }

    [MessagePackObject]
    public struct AgonesGameServerResponse
    {
        [Key(0)]
        public string Host { get; set; }
        
        [Key(1)]
        public int Port { get; set; }
    }
}
