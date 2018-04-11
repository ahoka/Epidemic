using System.Net;

namespace Epidemic.Protocol
{
    [MessagePack.Union(0, typeof(PingMessage))]
    [MessagePack.Union(1, typeof(PongMessage))]
    public interface IProtocolMessage
    {
    }
}