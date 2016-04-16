using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using TT.WSServer.DTO;

namespace TT.WSServer
{
    internal static class GlobalCache
    {
        internal static ConcurrentDictionary<Guid, IWebSocketConnection> AllSockets = new ConcurrentDictionary<Guid, IWebSocketConnection>();

        internal static readonly ConcurrentDictionary<string, IQuote> RealTimeUpdatePool = new ConcurrentDictionary<string, IQuote>();

        internal static readonly ConcurrentDictionary<string, IQuote> DelayedUpdatePool = new ConcurrentDictionary<string, IQuote>();

        /// <summary>
        /// RealTime Symbol to Connections
        /// </summary>
        internal static readonly ConcurrentDictionary<string, WatchInfo> RealTimeWatches = new ConcurrentDictionary<string, WatchInfo>();
        /// <summary>
        /// Delayed Symbol to Connections
        /// </summary>
        internal static readonly ConcurrentDictionary<string, WatchInfo> DelayedWatches = new ConcurrentDictionary<string, WatchInfo>();

        /// <summary>
        /// Connection (Guid) to Symbols
        /// </summary>
        internal static readonly ConcurrentDictionary<Guid, Subscription> ConnectionSubscriptions = new ConcurrentDictionary<Guid, Subscription>();

        internal static HashSet<int> OTCRealTimeUsers;
        internal static HashSet<string> OTCSymbols;

        static GlobalCache()
        {
            //TODO
            OTCSymbols = new HashSet<string>();
        }
    }
}
