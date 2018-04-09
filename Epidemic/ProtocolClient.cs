using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epidemic
{
    public class ProtocolClient : IDisposable
    {
        private Bootstrap bootstrap;
        private MultithreadEventLoopGroup group;

        public ProtocolClient()
        {
            group = new MultithreadEventLoopGroup();

            // Protocol:
            // --------------------------------
            // | len: 4 | messagepack payload |
            // --------------------------------

            bootstrap = new Bootstrap()
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;

                    pipeline.AddLast(new LoggingHandler("Incoming-Connect"));

                    pipeline.AddLast(new LengthFieldBasedFrameDecoder(128 * 1024, 0, 4, 0, 4));
                    pipeline.AddLast(new LengthFieldPrepender(4));

                    pipeline.AddLast(new MessagePackDecoder());
                    pipeline.AddLast(new MessagePackEncoder());

                    pipeline.AddLast(new MessageHandler());
                }));
        }

        public async Task<IChannel> Connect(Uri uri)
        {
            return await bootstrap.ConnectAsync(IPAddress.Parse(uri.DnsSafeHost), uri.Port);
        }

        public void Dispose()
        {
            group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
                .GetAwaiter().GetResult();
        }
    }
}
