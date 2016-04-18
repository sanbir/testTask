﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TT.DAL.Services;
using TT.WebSocketServer;

namespace TT.WSServer.Tests
{
    [TestClass]
    public class QuoteProviderTests
    {
        [TestMethod]
        public void Test()
        {
            var quoteProvider = new QuoteProvider(new Server());

            quoteProvider.Run();
        }
    }
}
