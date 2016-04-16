using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TT.WebSocketPublisher
{
    static class Program
    {
        static void Main(string[] args)
        {
            var service = new PublisherWinService();

            service.RunService(args);
        }
    }
}
