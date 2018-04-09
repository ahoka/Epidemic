using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Epidemic.Protocol;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic
{
    class MessagePackDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var message = MessagePackSerializer.Deserialize<IProtocolMessage>(input.Array);
            output.Add(message);
        }
    }
}
