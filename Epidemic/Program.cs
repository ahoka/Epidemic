using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
                //.MinimumLevel.Debug()
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
                        await server.BindAsync();

                        var channel = await client.Bind(new Uri("tcp://0.0.0.0:4011"));

                        await channel.WriteAndFlushAsync(new PingMessage(Guid.NewGuid()));

                        Console.ReadLine();

                        await channel.DisconnectAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex}");
                }
            }
        }
    }
}
