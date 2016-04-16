﻿using System;
using Fleck;

namespace TT.WSServer
{
    internal class ClientInfo
    {
        public ClientInfo(Guid connectionGuid, IWebSocketConnection connection)
        {
            ConnectionGuid = connectionGuid;
            Connection = connection;
        }

        public Guid ConnectionGuid { get; }
        public IWebSocketConnection Connection { get;}

        public String Filter { get; set;}

    }
}
