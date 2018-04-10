using DotNetty.Transport.Channels;
using Epidemic.Protocol;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic
{
    public class MessageHandler : SimpleChannelInboundHandler<IProtocolMessage>
    {
        private ILogger<MessageHandler> log;
        private Guid nodeId;

        public MessageHandler(ILogger<MessageHandler> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.nodeId = Guid.NewGuid();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, IProtocolMessage msg)
        {
            switch (msg)
            {
                case PingMessage ping:
                    log.Information($"Ping from {ping.NodeId}");
                    ctx.WriteAndFlushAsync(new PongMessage(nodeId)).Wait();
                    break;
                case PongMessage pong:
                    log.Information($"Pong from {pong.NodeId}");
                    break;
                default:
                    log.Warning("Unknown protocol message!");
                    break;
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            log.Error(exception, "Error during message handling");
        }
    }
}
