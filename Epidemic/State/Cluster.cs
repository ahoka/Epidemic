using Epidemic.State;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Epidemic.State
{
    public class Cluster
    {
        //public NodeRef Self { get; }
        public Dictionary<Guid, NodeInfo> Nodes { get; }

        public Cluster()
        {
            //Self = new NodeRef(NodeId.NewNodeId(), address);
            Nodes = new Dictionary<Guid, NodeInfo>();
        }

        public void HandlePong(IPEndPoint remoteAddress, Guid nodeId)
        {
            if (Nodes.TryGetValue(nodeId, out var node))
            {
                node.LastSeen = DateTime.UtcNow;
                node.State = NodeState.Healthy;
            }
            else
            {
                Nodes.Add(nodeId,
                    new NodeInfo(new NodeRef(new NodeId(nodeId), new NodeAddress(remoteAddress)),
                            DateTime.UtcNow, NodeState.Healthy));
            }
        }
    }
}
