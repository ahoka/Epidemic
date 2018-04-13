using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Epidemic.Behavior;
using Epidemic.Protocol;
using LanguageExt;
using LanguageExt.TypeClasses;
using Scrutor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace Epidemic
{
    public class GossipHandler : SimpleChannelInboundHandler<IAddressedEnvelope<ProtocolMessage>>
    {
        private ILogger<GossipHandler> log;
        private GossipBehavior behavior;

        public GossipHandler(ILogger<GossipHandler> log, GossipBehavior behavior)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IAddressedEnvelope<ProtocolMessage> msg)
        {
            Log.Debug(ctx.Name);
            Log.Debug($"Received Datagram: {msg.Sender} => {msg.Recipient}");

            var response = behavior.Behavior(msg.Content);

            Log.Debug($"Sending Datagram: {response} {ctx.Channel.LocalAddress} => {msg.Sender}");

            response.Match(m => ignore(ctx.WriteAsync(new DefaultAddressedEnvelope<ProtocolMessage>(m, msg.Sender))),
                           () => unit);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            log.Error(exception, "Error during message handling");
        }
    }
}
