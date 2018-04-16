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
        readonly Bootstrap bootstrap;
        readonly MultithreadEventLoopGroup group;
        readonly Config config;
        Task<IChannel> channel;

        public string Name { get; }

        public GossipServer(Config config, GossipHandler messageHandler)
        {
            this.config = config;
            Name = config.Name;
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

                    //pipeline.AddLast($"{Name} Logger", new SerilogLoggingHandler());
                    //pipeline.AddLast($"{Name} Frame Decoder", new LengthFieldBasedFrameDecoder(128 * 1024, 0, 4, 0, 4));
                    //pipeline.AddLast($"{Name} Frame Encoder", new LengthFieldPrepender(2));
                    pipeline.AddLast($"{Name} Payload Decoder", new MessagePackDecoder());
                    pipeline.AddLast($"{Name} Payload Encoder", new MessagePackEncoder());
                    pipeline.AddLast($"{Name} Message Handler", messageHandler);
                }));

        }

        public Task<IChannel> BindAsync()
        {
            if (channel != null)
            {
                throw new InvalidOperationException("The server is already running.");
            }
            channel = bootstrap.BindAsync(IPAddress.Any, config.Port);

            return channel;
        }

        public async Task StopAsync()
        {
            try
            {
                if (channel != null)
                {
                    var ch = await channel;
                    await ch.DisconnectAsync();
                }
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }
    }
}
