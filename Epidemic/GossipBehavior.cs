using Epidemic.Protocol;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageExt.Prelude;

namespace Epidemic
{
    public class GossipBehavior
    {
        private readonly Cluster cluster;
        private readonly Guid nodeId = Guid.NewGuid();
        private readonly ILogger<GossipBehavior> log;

        public GossipBehavior(Cluster cluster, ILogger<GossipBehavior> log)
        {
            this.cluster = cluster ?? throw new ArgumentNullException(nameof(cluster));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Option<IProtocolMessage> Behavior(IProtocolMessage message)
        {
            switch (message)
            {
                case PingMessage ping:
                    log.Information($"Ping from {ping.NodeId}");
                    return new PongMessage(nodeId);
                case PongMessage pong:
                    log.Information($"Pong from {pong.NodeId}");
                    return None;
                default:
                    log.Warning("Unknown protocol message!");
                    return None;
            }
        }
    }
}
