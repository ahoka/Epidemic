using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Protocol
{
    [MessagePackObject]
    public class AckMessage : ProtocolMessage
    {
        public AckMessage(IEnumerable<Node> members, Node sender, Node target)
            : base(members, sender)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        [Key(2)]
        public Node Target { get; }
    }
}
