using Microsoft.Practices.Unity;
using TT.DAL.Entity;
using TT.DAL.Repository;
using TT.DAL.Services;
using TT.WSServer;

namespace TT.WebSocketPublisher
{
    public class Unity
    {
        private static IUnityContainer _cont;

        public static IUnityContainer Container
        {
            get { return _cont ?? (_cont = BuildUnityContainer()); }
        }


        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IQuoteService, QuoteFetcherService>();

            container.RegisterType<IQuoteListener, QuoteListener>();

            container.RegisterType<IQuoteRepository, QuoteRepository>();

            return container;
        }
    }
}
