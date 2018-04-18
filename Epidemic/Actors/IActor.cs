using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Actors
{
    public interface IActor
    {
        Action<object> Receive { get; }
        void Become(Action<object> behavior);

        ActorRef Sender { get; }
        ActorRef Self { get; }
    }
}
