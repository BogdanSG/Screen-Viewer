using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ScreenViewer {

    public enum SCMessageType {

        Send_Client_Connection_Info = 0,
        Send_Client_IP_Ports_Info = 1,
        Client_Disconnected = 2,
        Server_Exit = 3,
        Invalid_Password = 4,
        Invalid_NickName = 5,
        Connection_OK = 6,
        Mouse_Click = 7,
        PublicMessage = 8,
        PrivateMessage = 9,
        ClientConnected_Message = 10,
        ClientDisconnected_Message = 11,
        All_Clients_Online = 12,
        SendFile = 13,
        SendFileName = 14,
        FileNotFound = 15,
        FileTCP_Port = 16,
        DeleteFileServer = 17,
        FileDeleted = 18,

    }//SCMessageType

    public class SCMessage {
        public SCMessageType MessageType { get; set; }

        public string Message { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string IP { get; set; }

        public int UDP_Video_Port { get; set; }

        public int UDP_Audio_Port { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public static byte[] SerializeJSON_ToBytes(SCMessage Message) {

            return Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(Message));

        }//SerializeJSON

        public static SCMessage DeserializeJSON_FromBytes(byte[] JSON_Message, int index, int count) {

            return new JavaScriptSerializer().Deserialize<SCMessage>(Encoding.UTF8.GetString(JSON_Message, index, count));

        }//SerializeJSON

    }//SCMessage

}//ScreenViewer
