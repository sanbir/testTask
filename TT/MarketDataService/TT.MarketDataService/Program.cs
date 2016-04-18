
using System.Reflection;
using System.ServiceProcess;


namespace TT.MarketDataService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var service = new MarketDataWinService();

            var onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
            onStartMethod.Invoke(service, new object[] { new string[] { } });


            /*
                        ServiceBase.Run(service);*/
        }
    }
}
