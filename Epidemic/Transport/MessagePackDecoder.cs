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
    public class MessagePackDecoder : MessageToMessageDecoder<IAddressedEnvelope<IByteBuffer>>
    {
        protected override void Decode(IChannelHandlerContext context, IAddressedEnvelope<IByteBuffer> message, List<object> output)
        {
            Log.Debug(context.Name);
            var input = message.Content;

            if (input.ReadableBytes > 0)
            {
                ProtocolMessage protocolMessage;

                if (input.HasArray)
                {
                    var segment = new ArraySegment<byte>(input.Array, input.ArrayOffset + input.ReaderIndex, input.ReadableBytes);
                    protocolMessage = MessagePackSerializer.Deserialize<ProtocolMessage>(segment);
                }
                else
                {
                    var buf = new byte[input.ReadableBytes];
                    input.ReadBytes(buf);
                    protocolMessage = MessagePackSerializer.Deserialize<ProtocolMessage>(buf);
                }

                output.Add(new DefaultAddressedEnvelope<ProtocolMessage>(protocolMessage, message.Sender, message.Recipient));
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Warning(exception, "Invalid protocol message received");
        }
    }
}
