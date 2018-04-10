﻿using System;
using System.Net;

namespace Epidemic
{
    public class Node
    {
        public Guid Id { get; set; }
        public IPAddress Address {get;set;}
        public DateTime LastSeen { get; set; }
        public NodeState State { get; set; }
    }
}