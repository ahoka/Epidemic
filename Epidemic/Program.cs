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

            using (var serviceProvider = services.BuildServiceProvider())
            {
                try
                {
                    using (var server = serviceProvider.GetRequiredService<ProtocolServer>())
                    using (var client = serviceProvider.GetRequiredService<ProtocolClient>())
                    {
                        await server.BindAsync();

                        var channel = await client.Connect(new Uri("tcp://127.0.0.1:4010"));

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
