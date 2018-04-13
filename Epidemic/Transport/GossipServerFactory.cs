using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Epidemic
{
    public class GossipServerFactory
    {
        IServiceProvider serviceProvider;

        public GossipServerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IGossipServer Create(string name, int port)
        {
            var handler = serviceProvider.GetService<GossipHandler>();
            return new GossipServer(name, port, handler);
        }
    }
}
