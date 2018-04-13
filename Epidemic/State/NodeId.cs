using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.State
{
    public class NodeId : NewType<NodeId, Guid>
    {
        public NodeId(Guid value) : base(value)
        {
        }

        public static NodeId NewNodeId() => new NodeId(Guid.NewGuid());

        public static implicit operator Guid(NodeId id)
        {
            return id;
        }
    }
}
