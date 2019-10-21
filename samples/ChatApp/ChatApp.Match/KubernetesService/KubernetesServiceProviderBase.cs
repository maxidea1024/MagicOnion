using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Match.KubernetesService
{
    public abstract class KubernetesServiceProviderBase : IKubernetesServiceProvider
    {
        public abstract string AccessToken { get; }
        public abstract string HostName { get; }
        public abstract string NameSpace { get; }
        public abstract string KubernetesServiceEndPoint { get; }
        public abstract bool IsRunningOnKubernetes { get; }

        public bool SkipCertificateValidation { get; set; } = false;

        public async Task<IGameServerInfo[]> GetGameServersAsync()
        {
            using (var httpClient = CreateHttpClient())
            {
                // Endpoints:
                // /apis/{APIGroup}/{VERSION}/{RESOURCE}/
                // /apis/agones.dev/v1/gameservers/

                var @namespace = NameSpace;
                var hostName = HostName;
                var gameServers = await GetGameServer(httpClient, $"/apis/agones.dev/v1/gameservers");
                var gameserverInfo = Utf8Json.JsonSerializer.Deserialize<IGameServerInfo[]>(gameServers);
                return gameserverInfo;
            }
        }

        public async Task AllocateGameServersAsync()
        {
            throw new NotImplementedException();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            if (SkipCertificateValidation)
            {
                bool AlwaysTrue(HttpRequestMessage r, X509Certificate2 c2, X509Chain c, System.Net.Security.SslPolicyErrors e) => true;
                httpClientHandler.ServerCertificateCustomValidationCallback = AlwaysTrue;
            }
            return httpClient;
        }

        private async Task<byte[]> GetGameServer(HttpClient httpClient, string apiPath)
        {
            var bytes = await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
            return bytes;
            //Utf8Json.JsonSerializer.DeserializeAsync<>
        }
    }
}
