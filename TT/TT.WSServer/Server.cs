using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json;
using TT.Core.Logger;
using TT.Core.Settings;
using TT.DAL.Pocos;
using TT.DAL.Services;
using TT.WSServer.DTO;

namespace TT.WSServer
{
    public class Server : IDisposable
    {
        private readonly IQuoteListener _quoteListener;
        private WebSocketServer _server;

        

        public Server()
        {
            _quoteListener = new QuoteListener(this);
            
            FleckLog.LogAction = OverrideFleckLogging;

            //HandleMessageFromClient(new WSClientMessage());
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

        public static readonly ConcurrentDictionary<Guid, ClientInfo> ClientInfo = new ConcurrentDictionary<Guid, ClientInfo>();

        public void Initialize()
        {
            string location = TTSettings.GetAppSetting<string>("hostingUrl", defaultVal: "ws://0.0.0.0:8080");
            _server = new WebSocketServer(location);

            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    ClientInfo.TryAdd(socket.ConnectionInfo.Id, new ClientInfo(socket.ConnectionInfo.Id,socket));
                    this.NotifySubscribers(_quoteListener.PreviousQuotes);
                };

                socket.OnError = OnError;

                socket.OnClose = () =>
                {
                    var found = ClientInfo.FirstOrDefault(ci => ci.Key == socket.ConnectionInfo.Id).Value;
                    if(found != null)
                    ClientInfo.TryRemove(socket.ConnectionInfo.Id, out found);
                };

                socket.OnMessage = message => OnMessageFromClient(message, socket);
            });

            _quoteListener.Listen();

        }

        public void NotifySubscribers( List<QuotePoco> quotes)
        {
            try
            {
                foreach (var clientInfo in ClientInfo.Values)
                {
                    
                    List<QuotePoco> quotesToUser;
                    if (String.IsNullOrEmpty(clientInfo.Filter))
                    {
                        quotesToUser = quotes;
                    }
                    else
                    {
                        quotesToUser = quotes.Where(quote => quote.Symbol.ToLower().Contains(clientInfo.Filter.ToLower())).ToList();
                    }
                    
                    string message = JsonConvert.SerializeObject(quotesToUser);

                    clientInfo.Connection.Send(message);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex.Message, ex);
            }
        }

        private void OnError(Exception x)
        {
            Logger.Current.Error(x.Message, x);
        }

        private void OnMessageFromClient(string requestMessage, IWebSocketConnection socket)
        {
            IPAddress ip;
            if (IPAddress.TryParse(socket.ConnectionInfo.ClientIpAddress, out ip))
            {
                WSClientMessage msg = new WSClientMessage()
                {
                    RawMessage = requestMessage,
                    ConnectionGuid = socket.ConnectionInfo.Id,
                    ClientIP = ip.GetAddressBytes(),
                    ClientIPString = socket.ConnectionInfo.ClientIpAddress
                };
                
            }
            else
                Logger.Current.Info(
                    $"Unable to parse IP {socket.ConnectionInfo.ClientIpAddress} from req msg {requestMessage}");
        }

      
       

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}
