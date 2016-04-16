using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace TT.WebSocketPublisher
{
    static class Program
    {
        private static void Main(string[] args)
        {
            //RegisterUnity();

            var service = new PublisherWinService();

            var onStartMethod = typeof (ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
                onStartMethod.Invoke(service, new object[] { new string[] { } });

            Console.ReadLine();
        }

        private static void RegisterUnity()
        {
            //Unity.Container.Resolve()
        }
    }
}
