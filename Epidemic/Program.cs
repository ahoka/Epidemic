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
using Epidemic.Behavior;
using Epidemic.Protocol;
using Epidemic.State;
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

            //services.Scan(scan => scan.FromEntryAssembly().AddClasses().AsSelf().AsImplementedInterfaces());
            services.AddSingleton<GossipServerFactory>();
            services.AddTransient<GossipHandler>();
            services.AddTransient<GossipBehavior>();
            services.AddTransient<Cluster>();

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                try
                {
                    var serverFactory = scope.ServiceProvider.GetRequiredService<GossipServerFactory>();

                    using (var server = serverFactory.Create("Server", 4011))
                    using (var client = serverFactory.Create("Client", 4010))
                    {
                        var serverChannel = await server.BindAsync(4010);

                        var clientChannel = await client.BindAsync(4011);
                        //var channel = await client.Connect(new Uri("tcp://127.0.0.1:4010"));
                        var message = new DefaultAddressedEnvelope<ProtocolMessage>(new PingMessage(Enumerable.Empty<NodeMessage>(), new NodeMessage(Guid.NewGuid(), new Uri("udp://127.0.0.1:4011"))),
                            new IPEndPoint(IPAddress.Loopback, 4011));
                        await clientChannel.WriteAndFlushAsync(message);
                        //var message = Encoding.UTF8.GetBytes("HELLO");
                        //var buf = Unpooled.WrappedBuffer(message);
                        //var datagram = new DatagramPacket(buf, new IPEndPoint(IPAddress.Loopback, 4010));
                        //await channel.WriteAndFlushAsync(datagram);

                        Console.ReadLine();
                        await clientChannel.DisconnectAsync();
                        await serverChannel.DisconnectAsync();
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
