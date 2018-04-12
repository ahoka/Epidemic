using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Epidemic.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Epidemic
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithDemystifiedStackTraces()
                .WriteTo.Async(l => l.Console())
                .CreateLogger();

            var services = new ServiceCollection();

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.Scan(scan => scan.FromEntryAssembly().AddClasses().AsSelf().AsImplementedInterfaces());
            //services.AddSingleton<ProtocolServer>();
            //services.AddSingleton<MessageHandler>();

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                try
                {
                    using (var server = scope.ServiceProvider.GetRequiredService<GossipServer>())
                    using (var client = scope.ServiceProvider.GetRequiredService<GossipClient>())
                    {
                        await server.BindAsync(4010);

                        var channel = await client.BindAsync(4011);
                        //var channel = await client.Connect(new Uri("tcp://127.0.0.1:4010"));
                        var message = new DefaultAddressedEnvelope<IProtocolMessage>(new PingMessage(Guid.NewGuid()), new IPEndPoint(IPAddress.Loopback, 4010));
                        await channel.WriteAndFlushAsync(message);
                        //var message = Encoding.UTF8.GetBytes("HELLO");
                        //var buf = Unpooled.WrappedBuffer(message);
                        //var datagram = new DatagramPacket(buf, new IPEndPoint(IPAddress.Loopback, 4010));
                        //await channel.WriteAndFlushAsync(datagram);

                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Program error");
                    Log.CloseAndFlush();
                }
            }
        }
    }
}
