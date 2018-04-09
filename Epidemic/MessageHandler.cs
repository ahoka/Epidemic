using DotNetty.Transport.Channels;
using Epidemic.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic
{
    class MessageHandler : SimpleChannelInboundHandler<IProtocolMessage>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, IProtocolMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine($"Exception: {exception.Message}");
        }
    }
}
