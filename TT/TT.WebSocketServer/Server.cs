using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace TT.WebSocketServer
{
    public class Server : IDisposable
    {
        private static Logger _logger = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Fleck.WebSocketServer _server;

        internal bool _isThrottle = false;
        private readonly System.Timers.Timer _throttleTimer;
        private readonly IncomingQuoteProcessor _quoteProcessor;
        private ProcessQueue<WSClientMessage> _msgFromClientQ;

        public Server()
        {
            int interval = AOS.Common.ConfigUtils.GetAppSetting<int>("DistributionIntervalInMS", defaultVal: 500, throwOnException: false);

            if (interval > 0)
            {
                _isThrottle = true;
                // Setup the update timer
                _throttleTimer = new System.Timers.Timer();
                _throttleTimer.Elapsed += ThrottleTimer_Elapsed;
                _throttleTimer.Interval = interval;
                _throttleTimer.Enabled = true;
            }

            FleckLog.LogAction = OverrideFleckLogging;

            _msgFromClientQ = new ProcessQueue<WSClientMessage>(HandleMessageFromClient, 1);
            _msgFromClientQ.Start();

            _quoteProcessor = new IncomingQuoteProcessor(_isThrottle);
        }

        private void OverrideFleckLogging(LogLevel level, string message, Exception ex)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    //_logger.Debug(message, ex); //Fleck prints a lot of DEBUG 
                    break;
                case LogLevel.Error:
                    _logger.Error(message, ex);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message, ex);
                    break;
                default:
                    _logger.Info(message, ex);
                    break;
            }
        }

        private void ThrottleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var c in GlobalCache.RealTimeUpdatePool)
            {
                WatchInfo watchInfo;

                if (GlobalCache.RealTimeWatches.TryGetValue(c.Key, out watchInfo))
                {
                    watchInfo.NotifySubscribers(c.Value);
                }
            }

            foreach (var c in GlobalCache.DelayedUpdatePool)
            {
                WatchInfo watchInfo;

                if (GlobalCache.DelayedWatches.TryGetValue(c.Key, out watchInfo))
                {
                    watchInfo.NotifySubscribers(c.Value);
                }
            }

            //if(_logger.IsDebugEnabled) _logger.Debug("Distributed symbols: " + GlobalCache.UpdatePool.Count);
            // Clear the pool
            GlobalCache.RealTimeUpdatePool.Clear(); //calling .Clear() can be expensive for CLR/GC depending on the size
            GlobalCache.DelayedUpdatePool.Clear();
        }

        public void Initialize()
        {
            string location = AOS.Common.ConfigUtils.GetAppSetting<string>("hostingUrl", defaultVal: "ws://0.0.0.0:8080");
            _server = new Fleck.WebSocketServer(location);

            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    ConnectionManager.OnOpen(socket);
                };

                socket.OnError = x => OnError(x);

                socket.OnClose = () =>
                {
                    ConnectionManager.OnClose(socket, _quoteProcessor);
                };

                socket.OnMessage = message => OnMessageFromClient(message, socket);
            });
        }

        private void OnError(Exception x)
        {
            _logger.Error(x);
        }

        private void OnMessageFromClient(string requestMessage, IWebSocketConnection socket)
        {
            //TODO: add some message validation here
            if (requestMessage == "" || !requestMessage.StartsWith("0=", StringComparison.Ordinal))
                return;

            IPAddress ip;
            if (IPAddress.TryParse(socket.ConnectionInfo.ClientIpAddress, out ip))
            {
                WSClientMessage msg = new WSClientMessage()
                {
                    RawMessage = requestMessage,
                    ConnectionGuid = socket.ConnectionInfo.Id,
                    ClientIP = ip.GetAddressBytes(),
                    ClientIPString = socket.ConnectionInfo.ClientIpAddress,
                    AuthCreatedTime = DateTime.Now
                };

                _msgFromClientQ.Enqueue(msg);
            }
            else
                _logger.Warn(string.Format("Unable to parse IP {0} from req msg {1}", socket.ConnectionInfo.ClientIpAddress, requestMessage));
        }

        private void HandleMessageFromClient(WSClientMessage clientMessage)
        {
            if (!AuthHelper.TryParseMessageAndAuthorize(ref clientMessage))
            {
                IWebSocketConnection conn;
                if (GlobalCache.AllSockets.TryGetValue(clientMessage.ConnectionGuid, out conn))
                {
                    //0 key is the "status code"
                    conn.Send("0=0|NOT AUTHORIZED");
                    conn.Close();
                    _logger.Warn("Request NOT AUTHORIZED " + clientMessage.RawMessage);
                }
                return;
            }

            ConnectionManager.OnSubscriptionMessage(ref clientMessage);

            _quoteProcessor.GetDataAndSubscribe(clientMessage.ConnectionGuid, clientMessage.SubscribedSymbols, clientMessage.IsUserRealTimeProvisioned);

            if (_logger.IsDebugEnabled) _logger.Debug(string.Format("Incoming req msg [{0}] from {1}", clientMessage.RawMessage, clientMessage.ConnectionGuid));
        }

        public void Dispose()
        {
            if (_server != null)
            {
                _msgFromClientQ.Dispose();
                _quoteProcessor.Dispose();

                _server.Dispose();
            }
        }
    }
}
