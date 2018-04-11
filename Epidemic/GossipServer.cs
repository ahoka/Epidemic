using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Epidemic.Protocol;
using Scrutor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epidemic
{
    public class GossipServer : IDisposable
    {
        private Bootstrap bootstrap;
        private MultithreadEventLoopGroup group;
        private IChannel boundChannel;

        public GossipServer(GossipHandler messageHandler)
        {
            group = new MultithreadEventLoopGroup();

            // Protocol:
            // --------------------------------
            // | len: 2 | messagepack payload |
            // --------------------------------

            bootstrap = new Bootstrap()
                .Group(group)
                .Channel<SocketDatagramChannel>()
                .Option(ChannelOption.SoBroadcast, true)
                .Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;

                    pipeline.AddLast("Server Frame Decoder", new LengthFieldBasedFrameDecoder(128 * 1024, 0, 4, 0, 4));
                    pipeline.AddLast("Server Frame Encoder", new LengthFieldPrepender(2));
                    pipeline.AddLast("Server Payload Decoder", new MessagePackDecoder());
                    pipeline.AddLast("Server Payload Encoder", new MessagePackEncoder());
                    pipeline.AddLast("Server Message Handler", messageHandler);
                }));
        }

        public async Task BindAsync(int port)
        {
            boundChannel = await bootstrap.BindAsync(IPAddress.Any, port);
        }

        public void Dispose()
        {
            try
            {
                if (boundChannel != null)
                {
                    boundChannel.CloseAsync().Wait();
                }
            }
            finally
            {
                group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)).Wait();
            }
        }
    }
}
