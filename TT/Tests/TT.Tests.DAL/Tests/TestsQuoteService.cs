using Microsoft.VisualStudio.TestTools.UnitTesting;
using TT.DAL.Services;

namespace TT.Tests.DAL.Tests
{
    [TestClass]
    public class TestsQuoteService
    {
        [TestMethod]
        public void TestGetQuoutes()
        {
           IQuoteService quoteService = new QuoteService();
           var quotes = quoteService.GetQuotes();
           Assert.AreNotEqual(0, quotes.Count);
        }
    }
}
