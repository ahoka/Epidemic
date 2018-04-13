using LanguageExt;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Epidemic.State
{
    sealed public class NodeAddress : NewType<NodeAddress, Uri>
    {
        public NodeAddress(Uri value) : base(value)
        {
        }

        public NodeAddress(IPEndPoint endPoint)
            : base(new Uri($"udp://{endPoint.Address}:{endPoint.Port}"))
        {
        }

        public static implicit operator Uri(NodeAddress address)
        {
            return address;
        }
    }
}
