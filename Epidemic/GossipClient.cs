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
    public class GossipClient : IDisposable
    {
        private Bootstrap bootstrap;
        private MultithreadEventLoopGroup group;

        public GossipClient(MessageHandler messageHandler)
        {
            group = new MultithreadEventLoopGroup();

            // Protocol:
            // --------------------------------
            // | len: 4 | messagepack payload |
            // --------------------------------

            bootstrap = new Bootstrap()
                .Group(group)
                .Channel<SocketDatagramChannel>()
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;

                    pipeline.AddLast(new LoggingHandler("Incoming-Connect"));

                    pipeline.AddLast("Client Frame Encoder", new LengthFieldPrepender(4));
                    pipeline.AddLast("Client Frame Decoder", new LengthFieldBasedFrameDecoder(128 * 1024, 0, 4, 0, 4));
                    pipeline.AddLast("Client Payload Encoder", new MessagePackEncoder());
                    pipeline.AddLast("Client Payload Decoder", new MessagePackDecoder());
                    pipeline.AddLast("Client Message Handler", messageHandler);
                }));
        }

        public async Task<IChannel> Bind(Uri uri)
        {
            return await bootstrap.BindAsync(IPAddress.Parse(uri.DnsSafeHost), uri.Port);
        }

        public void Dispose()
        {
            group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
                .GetAwaiter().GetResult();
        }
    }
}
