using System;
using System.Threading.Tasks;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Epidemic.Protocol;

namespace Epidemic
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                using (var server = new ProtocolServer())
                using (var client = new ProtocolClient())
                {
                    await server.BindAsync();

                    Console.ReadKey();

                    var channel = await client.Connect(new Uri("tcp://127.0.0.1:4010"));

                    await channel.WriteAndFlushAsync(new PingMessage(Guid.NewGuid()));
                    await channel.DisconnectAsync();

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
