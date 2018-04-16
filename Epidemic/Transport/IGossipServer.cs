using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;

namespace Epidemic
{
    public interface IGossipServer
    {
        string Name { get; }

        Task<IChannel> BindAsync();
        Task StopAsync();
    }
}