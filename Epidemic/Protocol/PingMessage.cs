using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Protocol
{
    [MessagePackObject]
    public class PingMessage : IProtocolMessage
    {
        public PingMessage(Guid nodeId)
        {
            NodeId = nodeId;
        }

        [Key(0)]
        public Guid NodeId { get; }
    }
}
