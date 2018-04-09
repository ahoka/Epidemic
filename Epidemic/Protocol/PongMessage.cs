using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Protocol
{
    public class PongMessage
    {
        public PongMessage(Guid nodeId)
        {
            NodeId = nodeId;
        }

        public Guid NodeId { get; }
    }
}
