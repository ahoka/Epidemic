using MessagePack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Epidemic.Protocol
{
    [MessagePackObject]
    public class PingMessage : ProtocolMessage
    {
        public PingMessage(IEnumerable<Node> members, Node sender)
            : base(members, sender)
        {
        }
    }
}
