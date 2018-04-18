using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Actors
{
    class Envelope
    {
        public Envelope(ActorRef sender, object message)
        {
            Sender = sender;
            Message = message;
        }

        public ActorRef Sender { get; }
        public object Message { get; }
    }
}
