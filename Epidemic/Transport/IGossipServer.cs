using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;

namespace Epidemic
{
    public interface IGossipServer : IDisposable
    {
        string Name { get; }

        Task<IChannel> BindAsync(int port);
    }
}