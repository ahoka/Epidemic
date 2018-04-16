using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Epidemic.Actors
{
    public abstract class Actor
    {
        BlockingCollection<object> mbox = new BlockingCollection<object>();

        public Actor()
        {
        }

        protected abstract Task OnMessage(object message);
    }
}
