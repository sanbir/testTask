using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using TT.DAL.Services;

namespace TT.UnitTests
{
    [TestClass]
    public class UnitTestsQuoteFetcherService
    {
        [TestMethod]
        public void TestGetQuoutes()
        {
           var quoteService = Unity.Container.Resolve<IQuoteService>();
           var quotes = quoteService.GetQuoutes();
        }
    }
}
