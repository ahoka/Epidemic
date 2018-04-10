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
    public class MessagePackDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            Log.Debug(context.Name);

            if (input.ReadableBytes > 0)
            {
                if (input.HasArray)
                {
                    var segment = new ArraySegment<byte>(input.Array, input.ArrayOffset + input.ReaderIndex, input.ReadableBytes);
                    output.Add(MessagePackSerializer.Deserialize<IProtocolMessage>(segment));
                }
                else
                {
                    var buf = new byte[input.ReadableBytes];
                    input.ReadBytes(buf);
                    output.Add(MessagePackSerializer.Deserialize<IProtocolMessage>(buf));
                }
            }
        }
    }
}
