using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using TT.Core.Logger;

namespace TT.WSServer.DTO
{
    /// <summary>
    /// Mapping of Symbol to connection
    /// </summary>
    internal sealed class WatchInfo
    {
        private readonly ConcurrentDictionary<Guid, IWebSocketConnection> _connections = new ConcurrentDictionary<Guid, IWebSocketConnection>();

        public ConcurrentDictionary<Guid, IWebSocketConnection> Connections => _connections;

        internal string Symbol { get; set; }

        /// <summary>
        /// Mapping of Symbol to connection
        /// </summary>
        internal WatchInfo(string symbol)
        {
            Symbol = symbol;
        }

        internal bool Active => _connections.Count > 0;

        internal void NotifySubscribers(IQuote message)
        {
            try
            {
                if (!Active)
                {
                    return;
                }

                string strMsg = message.ToString();

                foreach (var connectionId in _connections.Values)
                {
                    connectionId.Send(strMsg);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex.Message, ex);
            }
        }
    }

    internal interface IQuote
    {
        string ToString();
    }
}
