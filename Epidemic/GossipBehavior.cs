using Epidemic.Protocol;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static LanguageExt.Prelude;

namespace Epidemic
{
    public class GossipBehavior
    {
        private readonly Cluster cluster;
        private readonly Node nodeId = new Node(Guid.NewGuid(), new Uri("udp://127.0.0.1:4010"));
        private readonly ILogger<GossipBehavior> log;

        public GossipBehavior(Cluster cluster, ILogger<GossipBehavior> log)
        {
            this.cluster = cluster ?? throw new ArgumentNullException(nameof(cluster));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public static IEnumerable<NodeInfo> Members(Cluster cluster)
        {
            return cluster.Nodes.Values;
        }

        public Option<ProtocolMessage> Behavior(ProtocolMessage message)
        {
            var members = Members(cluster).Select(n => new Node(n.Id, new Uri($"udp://{n.Address}")));

            switch (message)
            {
                case PingMessage ping:
                    log.Information($"Ping from {ping.Sender.NodeId}");
                    return new AckMessage(members, nodeId, nodeId);
                case AckMessage pong:
                    log.Information($"Pong from {pong.Target.NodeId}");
                    return None;
                default:
                    log.Warning("Unknown protocol message!");
                    return None;
            }
        }
    }
}
