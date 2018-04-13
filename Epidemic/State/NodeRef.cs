using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.State
{
    sealed public class NodeRef : Record<NodeRef>
    {
        public NodeRef(NodeId id, NodeAddress address)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public NodeId Id { get; }
        public NodeAddress Address { get; }
    }
}
