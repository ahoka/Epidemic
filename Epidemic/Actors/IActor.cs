using System;
using System.Collections.Generic;
using System.Text;

namespace Epidemic.Actors
{
    public interface IActor
    {
        void OnMessage(object message);
    }
}
