using System;
using System.Net;
using Fleck;
using TT.Core.Logger;
using TT.Core.Settings;
using TT.WSServer.DTO;

namespace TT.WSServer
{
    public class Server : IDisposable
    {
        private static WebSocketServer _server;

        public Server()
        {
            FleckLog.LogAction = OverrideFleckLogging;

            HandleMessageFromClient(new WSClientMessage());
        }

        private void OverrideFleckLogging(LogLevel level, string message, Exception ex)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    //_logger.Debug(message, ex); //Fleck prints a lot of DEBUG 
                    break;
                case LogLevel.Error:
                    Logger.Current.Error(message, ex);
                    break;
                case LogLevel.Warn:
                    Logger.Current.Warning(message, ex);
                    break;
                default:
                    Logger.Current.Info(message);
                    break;
            }
        }

        public void Initialize()
        {
            string location = TTSettings.GetAppSetting<string>("hostingUrl", defaultVal: "ws://0.0.0.0:8080");
            _server = new WebSocketServer(location);

            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    ConnectionManager.OnOpen(socket);
                };

                socket.OnError = OnError;

                socket.OnClose = () =>
                {
                    ConnectionManager.OnClose(socket);
                };

                socket.OnMessage = message => OnMessageFromClient(message, socket);
            });
        }

        private void OnError(Exception x)
        {
            Logger.Current.Error(x.Message, x);
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
            }
            else
                Logger.Current.Info(
                    $"Unable to parse IP {socket.ConnectionInfo.ClientIpAddress} from req msg {requestMessage}");
        }

        private void HandleMessageFromClient(WSClientMessage clientMessage)
        {
            ConnectionManager.OnSubscriptionMessage(ref clientMessage);

            //_quoteProcessor.GetDataAndSubscribe(clientMessage.ConnectionGuid, clientMessage.SubscribedSymbols, clientMessage.IsUserRealTimeProvisioned);

            Logger.Current.Info($"Incoming req msg [{clientMessage.RawMessage}] from {clientMessage.ConnectionGuid}");
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}
