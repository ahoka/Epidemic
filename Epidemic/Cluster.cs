using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Epidemic
{
    public class Cluster
    {
        public Dictionary<Guid, Node> Nodes { get; }

        public Cluster()
        {
            Nodes = new Dictionary<Guid, Node>();
        }

        public void HandlePong(IPAddress remoteAddress, Guid nodeId)
        {
            if (Nodes.TryGetValue(nodeId, out var node))
            {
                if (node.Address != node.Address)
                {
                    Log.Warning($"Address changed for node {nodeId}: {node.Address} => {remoteAddress}");
                }

                node.LastSeen = DateTime.UtcNow;
                node.State = NodeState.Healthy;
            }
            else
            {
                Nodes.Add(nodeId, new Node()
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
