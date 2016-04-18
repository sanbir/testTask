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


namespace TT.WSServer
{
    public class Server : IDisposable
    {
        private readonly IQuoteProvider _quoteProvider;
        private WebSocketServer _server;

        
        public Server()
        {
            _quoteProvider = new QuoteProvider(this);
            IQuoteService quoteService  =new QuoteService();
            PreviousQuotes = quoteService.GetQuotes();
            FleckLog.LogAction = OverrideFleckLogging;
        }

        private void OverrideFleckLogging(LogLevel level, string message, Exception ex)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    // Logger.Current.Info(message);//Fleck prints a lot of DEBUG  
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

        public readonly ConcurrentDictionary<Guid, ClientInfo> ClientInfo = new ConcurrentDictionary<Guid, ClientInfo>();

        public void Initialize()
        {
            string location = TTSettings.GetAppSetting<string>("hostingUrl", defaultVal: "ws://0.0.0.0:8082");
            _server = new WebSocketServer(location);

            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    var clientInfo = new ClientInfo(socket.ConnectionInfo.Id, socket);
                    ClientInfo.TryAdd(socket.ConnectionInfo.Id, clientInfo);

                    var logMessage = "New client connected succesfully. id = " + clientInfo.ConnectionGuid;
                    Logger.Current.Info(logMessage);


                    NotifySubscriber(PreviousQuotes, clientInfo);
                    
                };

                socket.OnError = OnError;

                socket.OnClose = () =>
                {
                   
                    var found = ClientInfo.FirstOrDefault(ci => ci.Key == socket.ConnectionInfo.Id).Value;
                    if (found != null)
                    {
                        ClientInfo.TryRemove(socket.ConnectionInfo.Id, out found);
                        var logMessage = "Client disconnected. id = " + found.ConnectionGuid;
                        Logger.Current.Info(logMessage);
                    }
                };

                socket.OnMessage = message => OnMessageFromClient(message, socket);
            });

            Task.Run(() => _quoteProvider.Run());

        }

        public void NotifySubscriber(List<QuotePoco> quotesToSend, ClientInfo clientInfo)
        {
            try
            {
                List<QuotePoco> quotes = quotesToSend;
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

                var logMessage = "Data was sent to client = " + clientInfo.ConnectionGuid + ".\n Number of quotes is " + quotesToUser.Count;

                Logger.Current.Info(logMessage);

            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex.Message, ex);
            }
        }

        public void ProcessQuotes( List<QuotePoco> quotes)
        {
            try
            {
                if (ClientInfo.Values.Count < 1)
                {
                    return;
                }

                var quotesToSend = new List<QuotePoco>();

                foreach (var quote in quotes)
                {
                    var found = PreviousQuotes.FirstOrDefault(q => q.Symbol == quote.Symbol);
                    if (found != null)
                    {
                        if (!AreEqual(found, quote))
                        {
                            quotesToSend.Add(quote);
                        }
                    }
                    else
                    {
                        quotesToSend.Add(quote);
                    }
                }
                
                if (quotesToSend.Count < 1)
                    return;

                PreviousQuotes = quotes;

                NotifySubscribers(quotesToSend);
               
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex.Message, ex);
            }
        }

        private void NotifySubscribers(List<QuotePoco> quotesToSend)
        {
            try
            {
                foreach (var clientInfo in ClientInfo.Values)
                {
                    NotifySubscriber(quotesToSend, clientInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex.Message, ex);
            }
        }

        public List<QuotePoco> PreviousQuotes { get; private set; }

        private bool AreEqual(QuotePoco quote, QuotePoco quote2)
        {
            return quote.Ask == quote2.Ask && quote.Bid == quote2.Bid;
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
                if (ClientInfo.ContainsKey(socket.ConnectionInfo.Id))
                {
                    var clientInfo = ClientInfo[socket.ConnectionInfo.Id];

                    var oldFilter = clientInfo.Filter;
                    clientInfo.Filter = requestMessage;

                    var logMessage = "Client has applied a new filter. ClientID = " + clientInfo.ConnectionGuid +
                                     ". Filter = " + clientInfo.Filter;

                    Logger.Current.Info(logMessage);


                    if (String.IsNullOrEmpty(oldFilter) || clientInfo.Filter.ToLower().Contains(oldFilter.ToLower()))
                    {
                        return;
                    }
                    else
                    {
                        NotifySubscriber(PreviousQuotes, clientInfo);
                    }
                }
                else
                {
                    Logger.Current.Info(
                    $"Unable to find a client with connectionInfoID = {socket.ConnectionInfo.Id} from req msg {requestMessage}");
                }
                
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
