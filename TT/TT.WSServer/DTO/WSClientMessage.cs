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

        internal bool IsUserRealTimeProvisioned;

        internal Guid ConnectionGuid;

        internal Guid AuthorizationGuid;

        internal DateTime AuthCreatedTime;

        internal byte[] ClientIP;

        internal string ClientIPString;

        public override string ToString()
        {
            return
                $"Auth: {AuthorizationGuid} Conn: {ConnectionGuid} TS: {AuthCreatedTime} IP: {ClientIP} RealTime: {IsUserRealTimeProvisioned} Msg: {RawMessage}";
        }
    }
}
