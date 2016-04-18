using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TT.DAL.Services;

namespace TT.UnitTests.DAL
{
    [TestClass]
    public class TestsQuoteService
    {
        [TestMethod]
        public void TestGetQuoutes()
        {
           var quoteService = Unity.Container.Resolve<IQuoteService>();
           var quotes = quoteService.GetQuotes();
           Assert.AreNotEqual(0, quotes.Count);
        }
    }
}
