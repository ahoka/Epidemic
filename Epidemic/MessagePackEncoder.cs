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
    class MessagePackEncoder : MessageToByteEncoder<IProtocolMessage>
    {
        protected override void Encode(IChannelHandlerContext context, IProtocolMessage message, IByteBuffer output)
        {
            var binary = MessagePackSerializer.Serialize(message);

            output.WriteBytes(binary);
        }
    }
}
