using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Protocol
{
    class PingMessage : IProtocolMessage
    {
        public PingMessage(Guid nodeId)
        {
            NodeId = nodeId;
        }

        public Guid NodeId { get; }
    }
}
