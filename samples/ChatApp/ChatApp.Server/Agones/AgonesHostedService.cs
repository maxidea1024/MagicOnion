using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MagicOnion.Agones
{
    public class AgonesHostedService : IHostedService
    {
        readonly IAgonesSdk _agonesSdk;
        readonly ILogger<AgonesHostedService> _logger;
        public AgonesHostedService(IAgonesSdk agonesSdk, ILogger<AgonesHostedService> logger)
        {
            _agonesSdk = agonesSdk;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now}: Starting Agones Health Ping");
            // fire and forget
            _agonesSdk.StartAsync(cancellationToken)
                .ContinueWith(x => _logger.LogError($"Task Unhandled {x.Exception}"), TaskContinuationOptions.OnlyOnFaulted);
            _agonesSdk.Ready();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _agonesSdk.StopAsync();
        }
    }
}
