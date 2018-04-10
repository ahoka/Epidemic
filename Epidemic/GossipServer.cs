using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Scrutor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidemic
{
    public class GossipServer : IDisposable
    {
        private ServerBootstrap bootstrap;
        private MultithreadEventLoopGroup group;
        private IChannel boundChannel;

        public GossipServer(MessageHandler messageHandler)
        {
            group = new MultithreadEventLoopGroup(1);

            // Protocol:
            // --------------------------------
            // | len: 4 | messagepack payload |
            // --------------------------------

            bootstrap = new ServerBootstrap()
                .Group(group)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .Handler(new LoggingHandler("Incoming-Listener"))
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;

                    pipeline.AddLast(new LoggingHandler("Incoming-Connect"));

                    pipeline.AddLast("Server Frame Decoder", new LengthFieldBasedFrameDecoder(128 * 1024, 0, 4, 0, 4));
                    pipeline.AddLast("Server Frame Encoder", new LengthFieldPrepender(4));
                    pipeline.AddLast("Server Payload Decoder", new MessagePackDecoder());
                    pipeline.AddLast("Server Payload Encoder", new MessagePackEncoder());
                    pipeline.AddLast("Server Message Handler", messageHandler);
                }));
        }

        public async Task BindAsync()
        {
            boundChannel = await bootstrap.BindAsync(4010);
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
