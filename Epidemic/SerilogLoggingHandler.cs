using DotNetty.Transport.Channels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epidemic
{
    public class SerilogLoggingHandler : ChannelHandlerAdapter
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: ChannelActive");

            context.FireChannelActive();
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: ChannelInactive");

            context.FireChannelInactive();
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Log.Debug($"{context.Name}: ChannelRead {message}");

            context.FireChannelRead(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: ChannelReadComplete");

            context.FireChannelReadComplete();
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: ChannelRegistered");

            context.FireChannelRegistered();
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: ChannelUnregistered");

            context.FireChannelUnregistered();
        }

        public override void ChannelWritabilityChanged(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: ChannelWritabilityChanged");

            context.FireChannelWritabilityChanged();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Debug($"{context.Name}: ExceptionCaught {exception}");

            context.FireExceptionCaught(exception);
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            Log.Debug($"{context.Name}: UserEventTriggered {evt}");

            context.FireUserEventTriggered(evt);
        }

        public override Task BindAsync(IChannelHandlerContext context, EndPoint localAddress)
        {
            Log.Debug($"{context.Name}: BindAsync {localAddress}");

            return context.BindAsync(localAddress);
        }

        public override Task CloseAsync(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: CloseAsync");

            return context.CloseAsync();
        }

        public override Task ConnectAsync(IChannelHandlerContext context, EndPoint remoteAddress, EndPoint localAddress)
        {
            Log.Debug($"{context.Name}: ConnectAsync {remoteAddress} {localAddress}");

            return context.ConnectAsync(remoteAddress, localAddress);
        }

        public override Task DeregisterAsync(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: DeregisterAsync");

            return context.DeregisterAsync();
        }

        public override Task DisconnectAsync(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: DisconnectAsync");

            return context.DisconnectAsync();
        }

        public override void Flush(IChannelHandlerContext context)
        {
            Log.Debug($"{context.Name}: Flush");

            context.Flush();
        }

        public override Task WriteAsync(IChannelHandlerContext context, object message)
        {
            Log.Debug($"{context.Name}: WriteAsync {message}");

            return context.WriteAsync(message);
        }
    }
}
