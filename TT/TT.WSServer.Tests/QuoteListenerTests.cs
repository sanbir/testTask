using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TT.WSServer.Tests
{
    [TestClass]
    public class QuoteListenerTests
    {
        [TestMethod]
        public void TestListen()
        {
            var quoteListener = new QuoteListener(new Server());

            quoteListener.Listen();
        }
    }
}
