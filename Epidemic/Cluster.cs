using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Epidemic
{
    public class Cluster
    {
        public Dictionary<Guid, NodeInfo> Nodes { get; }

        public Cluster()
        {
            Nodes = new Dictionary<Guid, NodeInfo>();
        }

        public void HandlePong(IPAddress remoteAddress, Guid nodeId)
        {
            if (Nodes.TryGetValue(nodeId, out var node))
            {
                if (node.Address != remoteAddress)
                {
                    Log.Warning($"Address changed for node {nodeId}: {node.Address} => {remoteAddress}");
                }

                node.LastSeen = DateTime.UtcNow;
                node.State = NodeState.Healthy;
            }
            else
            {
                Nodes.Add(nodeId, new NodeInfo()
                {
                    Id = nodeId,
                    Address = remoteAddress,
                    LastSeen = DateTime.UtcNow,
                    State = NodeState.Healthy
                });
            }
        }
    }
}
