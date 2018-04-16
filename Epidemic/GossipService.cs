using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Epidemic
{
    public class GossipService : IHostedService
    {
        readonly IGossipServer server;
        readonly ILogger<GossipService> log;

        public GossipService(IGossipServer server, ILogger<GossipService> log)
        {
            this.log = log;
            this.server = server;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            log.Information("Gossip Service starting");
            return server.BindAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            log.Information("Gossip Service stopping");
            return server.StopAsync();
        }
    }
}