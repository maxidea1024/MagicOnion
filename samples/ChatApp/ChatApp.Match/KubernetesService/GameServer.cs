using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Match.KubernetesService
{
    public class GameServer
    {
        // lock
        static readonly object _syncObject = new object();

        static IKubernetesServiceProvider _serviceProvider = GetDefaultProvider();
        static IGameServerInfo[] _current;

        public static IGameServerInfo[] Current
        {
            get
            {
                if (_current == null)
                {
                    Initialize();
                }
                return _current;
            }
        }

        public static void Initialize(bool throwOnFail = false)
        {
            lock (_syncObject)
            {
                if (_current != null) return;

                if (!_serviceProvider.IsRunningOnKubernetes)
                {
                    _current = new[] { new PseudoGameServerInfo() };
                    return;
                }

                try
                {
                    _current = _serviceProvider.GetGameServersAsync().GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    if (throwOnFail)
                    {
                        throw;
                    }
                    else
                    {
                        _current = new[] { new PseudoGameServerInfo() };
                    }
                }
            }
        }

        public static void RegisterServiceProvider(IKubernetesServiceProvider provider)
            => _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));

        private static IKubernetesServiceProvider GetDefaultProvider()
        {
            return (Environment.OSVersion.Platform == PlatformID.Unix)
                ? (IKubernetesServiceProvider)new UnixKubernetesServiceProvider()
                : throw new NotImplementedException("Windows is not supported");
        }
    }
}
