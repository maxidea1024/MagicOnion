using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Match.KubernetesService
{
    public interface IGameServerInfo
    {
        bool IsRunningOnKubernetes { get; }
        string Host { get; }
        string Port { get; }
    }

    internal class GameServerInfo : IGameServerInfo
    {
        public bool IsRunningOnKubernetes => true;

        public string Host => throw new NotImplementedException();

        public string Port => throw new NotImplementedException();
    }

    internal class PseudoGameServerInfo : IGameServerInfo
    {
        static readonly IReadOnlyDictionary<string, string> EmptyStringDictionary = new Dictionary<string, string>();

        public bool IsRunningOnKubernetes => false;
        public string Namespace => "unknown";
        public string Name => Environment.MachineName;
        public string NodeName => Environment.MachineName;
        public string HostIP => null;
        public string PodIP => null;
        public IReadOnlyDictionary<string, string> Annotations => EmptyStringDictionary;
        public IReadOnlyDictionary<string, string> Labels => EmptyStringDictionary;

        public string Host => Environment.MachineName;

        public string Port => throw new NotImplementedException();

        public override string ToString()
        {
            return $"{Namespace}/{Name} @ {NodeName}";
        }
    }
}
