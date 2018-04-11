using Epidemic.Protocol;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic
{
    public class GossipBehavior
    {
        private Cluster cluster;
        private Guid nodeId;
        private ILogger<GossipBehavior> log;

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
                    break;
                default:
                    log.Warning("Unknown protocol message!");
                    break;
            }
        }
    }
}
