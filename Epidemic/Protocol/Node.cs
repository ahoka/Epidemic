using MessagePack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Epidemic.Protocol
{
    [MessagePackObject]
    public class Node
    {
        public Node(Guid nodeId, Uri address)
        {
            NodeId = nodeId;
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        [Key(0)]
        public Guid NodeId { get; }
        [Key(1)]
        public Uri Address { get; }
    }
}
