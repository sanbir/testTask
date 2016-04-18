using System.Reflection;
using System.ServiceProcess;

namespace TT.WSSWinService
{
    static class Program
    {
        private static void Main(string[] args)
        {
           
            var service = new WebSocketServerWinService();

           /* var onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
            onStartMethod.Invoke(service, new object[] { new string[] { } });
       */
            
            ServiceBase.Run(service);
        }

    }
}
