using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Core.Logger;
using TT.WSServer;

namespace TT.WebSocketPublisher
{
    public class Bootstrapper : IDisposable
    {
        private static Server _fleckServer;

        public Bootstrapper()
        {
            _fleckServer = new Server();
            _fleckServer.Initialize();
        }

        public void Dispose()
        {
            if (_fleckServer != null)
                _fleckServer.Dispose();
        }
    }
}
