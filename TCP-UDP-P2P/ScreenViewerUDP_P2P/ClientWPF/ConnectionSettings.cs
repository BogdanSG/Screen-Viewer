using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ClientWPF {
    public class ConnectionSettings {

        public string IP { get; set; }

        public int Port { get; set; }

        public bool ShowBrokenImage { get; set; }

        public bool RandomSend { get; set; }

        public int VideoSendPort { get; set; }

        public int AudioSendPort { get; set; }

        public string NickName { get; set; }

        public string ServerPassword { get; set; }

        public static void SerializeJSON(ConnectionSettings connectionSettings, string FName) {

            File.WriteAllText(FName, new JavaScriptSerializer().Serialize(connectionSettings));

        }//SerializeJSON

        public static ConnectionSettings DeserializeJSON(string FName) {

            return new JavaScriptSerializer().Deserialize<ConnectionSettings>(File.ReadAllText(FName));

        }//SerializeJSON


    }//ConnectionSettings

}//ClientWPF
