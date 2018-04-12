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
    public class GossipServer : IGossipServer
    {
        private Bootstrap bootstrap;
        private MultithreadEventLoopGroup group;

        public string Name { get; }

        public GossipServer(string name, GossipHandler messageHandler)
        {
            Name = name;
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

                    pipeline.AddLast($"{Name} Logger", new SerilogLoggingHandler());
                    //pipeline.AddLast($"{Name} Frame Decoder", new LengthFieldBasedFrameDecoder(128 * 1024, 0, 4, 0, 4));
                    //pipeline.AddLast($"{Name} Frame Encoder", new LengthFieldPrepender(2));
                    pipeline.AddLast($"{Name} Payload Decoder", new MessagePackDecoder());
                    pipeline.AddLast($"{Name} Payload Encoder", new MessagePackEncoder());
                    pipeline.AddLast($"{Name} Message Handler", messageHandler);
                }));
        }

        public async Task<IChannel> BindAsync(int port)
        {
            return await bootstrap.BindAsync(IPAddress.Any, port);
        }

        public void Dispose()
        {
            group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)).Wait();
        }
    }
}
