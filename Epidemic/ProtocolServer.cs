﻿using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Epidemic
{
    public class ProtocolServer : IDisposable
    {
        private ServerBootstrap bootstrap;
        private MultithreadEventLoopGroup bossGroup;
        private MultithreadEventLoopGroup workerGroup;
        private IChannel boundChannel;

        public ProtocolServer()
        {

            bossGroup = new MultithreadEventLoopGroup(1);
            workerGroup = new MultithreadEventLoopGroup();

            // Protocol:
            // --------------------------------
            // | len: 4 | messagepack payload |
            // --------------------------------

            try
            {
                bootstrap = new ServerBootstrap()
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("Incoming-Listener"))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
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
            finally
            {
                bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
                workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        public async Task BindAsync()
        {
            boundChannel = await bootstrap.BindAsync(4010);

        }

        public void Dispose()
        {
            await boundChannel.CloseAsync();

            Task.WhenAll(bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                         workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
        }
    }
}