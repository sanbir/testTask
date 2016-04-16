using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.WSServer.DTO
{
    internal class WSClientMessage
    {
        internal string RawMessage;

        internal string[] SubscribedSymbols;

        internal Guid ConnectionGuid;

        internal byte[] ClientIP;

        internal string ClientIPString;

        public override string ToString()
        {
            return
                $"Conn: {ConnectionGuid} IP: {ClientIP} Msg: {RawMessage}";
        }
    }
}
