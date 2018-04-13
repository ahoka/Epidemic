using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Protocol
{
    [MessagePackObject]
    public class PingReqMessage : ProtocolMessage
    {
        public PingReqMessage(IEnumerable<NodeMessage> members, NodeMessage sender, NodeMessage target)
            : base(members, sender)
        {
            Target = target;
        }

        [Key(2)]
        public NodeMessage Target { get; }
    }
}
