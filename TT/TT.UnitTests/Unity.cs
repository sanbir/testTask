using Microsoft.Practices.Unity;
using TT.DAL.Entity;
using TT.DAL.Repository;
using TT.DAL.Services;

namespace TT.UnitTests
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

            container.RegisterType<BaseMongoRepository<QuoteEntity>, QuoteRepository>();
            container.RegisterType<IQuoteService, QuoteFetcherService>();

            return container;
        }
    }
}
