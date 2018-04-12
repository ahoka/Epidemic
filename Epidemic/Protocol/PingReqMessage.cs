using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Protocol
{
    [MessagePackObject]
    public class PingReqMessage : ProtocolMessage
    {
        public PingReqMessage(IEnumerable<Node> members, Node sender, Node target)
            : base(members, sender)
        {
            Target = target;
        }

        [Key(2)]
        public Node Target { get; }
    }
}
