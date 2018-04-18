using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Epidemic.Actors
{
    public abstract class Actor : IActor
    {
        BlockingCollection<object> mbox = new BlockingCollection<object>();
        Action<object> currentBehavior;

        public Actor()
        {

        }

        public Action<object> Receive { get; protected set; }

        public ActorRef Sender => new ActorRef();

        public ActorRef Self => new ActorRef();

        public void Become(Action<object> behavior)
        {
            currentBehavior = behavior;
        }
    }
}
