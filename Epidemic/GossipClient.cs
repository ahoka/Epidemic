using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Epidemic.Protocol;
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

        public GossipClient(GossipHandler messageHandler)
        {
            group = new MultithreadEventLoopGroup();

            // Protocol:
            // --------------------------------
            // | len: 4 | messagepack payload |
            // --------------------------------

            bootstrap = new Bootstrap()
                .Group(group)
                .Channel<SocketDatagramChannel>()
                .Option(ChannelOption.SoBroadcast, true)
                .Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;

                    //pipeline.AddLast("Client Frame Encoder", new LengthFieldPrepender(4));
                    //pipeline.AddLast("Client Frame Decoder", new LengthFieldBasedFrameDecoder(128 * 1024, 0, 4, 0, 4));
                    pipeline.AddLast("Client Payload Encoder", new DatagramPacketEncoder<IProtocolMessage>(new MessagePackEncoder()));
                    pipeline.AddLast("Client Payload Decoder", new DatagramPacketDecoder(new MessagePackDecoder()));
                    pipeline.AddLast("Client Message Handler", messageHandler);
                }));
        }

        public async Task<IChannel> BindAsync(int port)
        {
            return await bootstrap.BindAsync(IPAddress.Any, port);
        }

        public async Task<IChannel> Connect(Uri uri)
        {
            return await bootstrap.ConnectAsync(uri.Host, uri.Port);
        }

        public void Dispose()
        {
            group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
                .GetAwaiter().GetResult();
        }
    }
}
