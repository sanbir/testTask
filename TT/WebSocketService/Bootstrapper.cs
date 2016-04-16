using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.WebSocketServer;

namespace TT.WebSocketPublisher
{
    public class Bootstrapper : IDisposable
    {
        private static Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Server _fleckServer;

        public Bootstrapper()
        {
            _fleckServer = new Server();
            _fleckServer.Initialize();
        }

        public void Dispose()
        {
            _fleckServer?.Dispose();
        }
    }
}
