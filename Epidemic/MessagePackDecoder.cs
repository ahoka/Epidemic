﻿using DotNetty.Buffers;
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
    class MessagePackDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            Log.Debug(context.Name);

            if (input.ReadableBytes > 0)
            {
                var buf = new byte[input.ReadableBytes];
                input.ReadBytes(buf);
                var message = MessagePackSerializer.Deserialize<IProtocolMessage>(buf);
                output.Add(message);
            }
        }
    }
}
