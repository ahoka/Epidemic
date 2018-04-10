using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Epidemic.Protocol;
using MessagePack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic
{
    class MessagePackEncoder : MessageToMessageEncoder<IProtocolMessage>
    {
        protected override void Encode(IChannelHandlerContext context, IProtocolMessage message, List<object> output)
        {
            Log.Debug(context.Name);

            var binary = MessagePackSerializer.Serialize(message);
            var buffer = context.Allocator.Buffer(binary.Length);
            buffer.WriteBytes(binary);

            output.Add(buffer);
        }
    }
}
