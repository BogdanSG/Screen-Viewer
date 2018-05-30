using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerWF {
    public enum ConnectionTypes {

        None = 0,
        LAN = 1,
        WAN = 2

    }//ConnectionTypes

    public class ClientInfo {

        public IPEndPoint VideoEndpoint { get; set; }

        public IPEndPoint AudioEndpoint { get; set; }

        public ConnectionTypes ConnectionType { get; set; }

        public TcpClient Client { get; set; }

        public string NickName { get; set; }

    }//ClientInfo

}//ServerWF
