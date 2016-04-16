using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;
using TT.Core.Logger;
using TT.WSServer.DTO;

namespace TT.WSServer
{
    internal static class ConnectionManager
    {
        internal static void OnOpen(IWebSocketConnection socket)
        {
            if (GlobalCache.AllSockets.TryAdd(socket.ConnectionInfo.Id, socket))
                Logger.Current.Info("Accepted new connection from " + socket.ConnectionInfo.Id);
        }

        internal static void OnClose(IWebSocketConnection socket)
        {
            IWebSocketConnection conn;
            if (GlobalCache.AllSockets.TryRemove(socket.ConnectionInfo.Id, out conn))
            {
                Subscription watches;
                if (GlobalCache.ConnectionSubscriptions.TryRemove(socket.ConnectionInfo.Id, out watches))
                {
                    Logger.Current.Info("Removed connection from " + socket.ConnectionInfo.Id);

                    List<string> unsubscribeCandidates = new List<string>();
                    foreach (var info in watches.Subscribtions)
                    {
                        if (info.Connections.TryRemove(socket.ConnectionInfo.Id, out conn))
                            if (info.Connections.IsEmpty)
                                unsubscribeCandidates.Add(info.Symbol);
                    }

                    if (unsubscribeCandidates.Count > 0)
                    {
                        foreach (string symbol in unsubscribeCandidates)
                        {
                            WatchInfo wi;
                            GlobalCache.RealTimeWatches.TryRemove(symbol, out wi);
                            GlobalCache.DelayedWatches.TryRemove(symbol, out wi);
                        }
                    }
                }
            }
            else
            {
                Logger.Current.Info("Closing unmonitored connection " + socket.ConnectionInfo.Id);
                Subscription watches;
                if (!GlobalCache.ConnectionSubscriptions.TryRemove(socket.ConnectionInfo.Id, out watches))
                    Logger.Current.Info("ConnectionSubscriptions removed " + socket.ConnectionInfo.Id);
                    else
                    Logger.Current.Info("ConnectionSubscriptions does not contain " + socket.ConnectionInfo.Id);
            }
        }

        internal static void OnSubscriptionMessage(ref WSClientMessage clientMessage)
        {
            //remove all other subscriptions for this Guid (do not allow adding to the existing subscriptions)
            Subscription subs;
            if (GlobalCache.ConnectionSubscriptions.TryGetValue(clientMessage.ConnectionGuid, out subs))
            {
                foreach (WatchInfo symbol in subs.Subscribtions)
                {
                    IWebSocketConnection conn;
                    symbol.Connections.TryRemove(clientMessage.ConnectionGuid, out conn);
                }

                //clear the subscribed symbols for this connection Guid
                subs.Subscribtions.Clear();
            }
            else
            {
                //add blank list of subscribed symbols

                Subscription subscr = new Subscription()
                {
                    AuthorizationGuid = clientMessage.AuthorizationGuid,
                    ConnectionGuid = clientMessage.ConnectionGuid,
                    Subscribtions = new List<WatchInfo>(),
                    AuthCreatedTime = clientMessage.AuthCreatedTime,
                    ClientIP = clientMessage.ClientIP,
                    IsAuthorized = true
                };
                GlobalCache.ConnectionSubscriptions.TryAdd(clientMessage.ConnectionGuid, subscr);
            }
        }
    }
}
