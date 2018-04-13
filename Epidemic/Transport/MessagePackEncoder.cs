using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Epidemic.Protocol;
using MessagePack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic
{
    class MessagePackEncoder : MessageToMessageEncoder<IAddressedEnvelope<ProtocolMessage>>
    {
        protected override void Encode(IChannelHandlerContext context, IAddressedEnvelope<ProtocolMessage> message, List<object> output)
        {
            Log.Debug(context.Name);

            var binary = MessagePackSerializer.SerializeUnsafe(message.Content);
            var buffer = context.Allocator.Buffer(binary.Count);
            buffer.WriteBytes(binary.Array, binary.Offset, binary.Count);

            output.Add(new DefaultAddressedEnvelope<IByteBuffer>(buffer, message.Recipient, message.Recipient));
        }
    }
}
