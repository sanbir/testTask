using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TT.DAL.Pocos;
using TT.DAL.Repository;
using TT.DAL.Services;

namespace TT.UnitTests.DAL
{
    [TestClass]
    public class UnitTestsQuoteRepository
    {
        private const string Symbol = "EURUSD";

        [TestMethod]
        public void Test()
        {
            var quoteService = Unity.Container.Resolve<IQuoteRepository>();
            var time = DateTime.UtcNow.AddHours(-2);

            var quotes = new List<QuotePoco>
            {
                new QuotePoco
                {
                    Time = time,
                    Bid = 42.4M,
                    Ask = 9000M,
                    ChangePercent = 42,
                    ChangePoints = 100500,
                    Symbol = Symbol
                },
                new QuotePoco
                {
                    Time = time,
                    Bid = 50M,
                    Ask = 9000M,
                    ChangePercent = 42,
                    ChangePoints = 100500,
                    Symbol = Symbol
                },
            };

            quoteService.Add(quotes);

            var fromDb = quoteService.Get(Symbol).Where(poco => (poco.Time - time).Duration() < new TimeSpan(0,0,0,1)).ToList();

            Assert.IsNotNull(fromDb.FirstOrDefault());

            Assert.AreEqual(fromDb.Count, 2);

            quoteService.RemoveUntil(time.AddMinutes(10));

            fromDb = quoteService.Get(Symbol).Where(poco => (poco.Time - time).Duration() < new TimeSpan(0, 0, 0, 1)).ToList();

            Assert.IsNull(fromDb.FirstOrDefault());
        }
    }
}
