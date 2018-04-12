using MessagePack;
using System.Collections.Generic;
using System.Net;

namespace Epidemic.Protocol
{
    [MessagePack.Union(0, typeof(PingMessage))]
    [MessagePack.Union(1, typeof(AckMessage))]
    [MessagePack.Union(2, typeof(PingReqMessage))]
    public abstract class ProtocolMessage
    {
        protected ProtocolMessage(IEnumerable<Node> members, Node sender)
        {
            Members = members ?? throw new System.ArgumentNullException(nameof(members));
            Sender = sender ?? throw new System.ArgumentNullException(nameof(sender));
        }

        [Key(0)]
        public IEnumerable<Node> Members { get; }

        [Key(1)]
        public Node Sender { get; }
    }
}