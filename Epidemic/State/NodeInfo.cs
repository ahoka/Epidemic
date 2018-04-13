using Epidemic.State;
using System;
using System.Net;

namespace Epidemic
{
    public class NodeInfo
    {
        public NodeInfo(NodeRef reference, DateTime lastSeen, NodeState state)
        {
            Reference = reference;
            LastSeen = lastSeen;
            State = state;
        }

        public NodeRef Reference { get; set; }
        public DateTime LastSeen { get; set; }
        public NodeState State { get; set; }
    }
}