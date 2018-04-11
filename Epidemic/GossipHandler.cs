using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Epidemic.Protocol;
using Scrutor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic
{
    public class GossipHandler : SimpleChannelInboundHandler<IProtocolMessage>
    {
        private ILogger<MessageHandler> log;
        private GossipBehavior behavior;

        public GossipHandler(ILogger<MessageHandler> log, GossipBehavior behavior)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IProtocolMessage msg)
        {
            Log.Debug(ctx.Name);

            Log.Information($"Message from: {ctx.Channel.RemoteAddress}");

            ctx.WriteAndFlushAsync().Wait();

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            log.Error(exception, "Error during message handling");
        }
    }
}
