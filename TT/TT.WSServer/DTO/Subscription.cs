using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.WSServer.DTO
{
    internal class Subscription
    {
        internal Guid AuthorizationGuid { get; set; }

        internal Guid ConnectionGuid { get; set; }

        internal List<WatchInfo> Subscribtions { get; set; }

        internal DateTime AuthCreatedTime { get; set; }

        internal byte[] ClientIP { get; set; }

        internal bool IsAuthorized { get; set; }
    }
}
