using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ServerWF {
    class SVServerSettings {

        public string IP { get; set; }

        public int TCP_Port { get; set; }

        public int VideoIndex { get; set; }

        public string Audio { get; set; }

        public int Video_UPD_Port { get; set; }

        public int FileSend_TCP_Port { get; set; }

        public bool AudioEnable { get; set; }

        public int Audio_UPD_Port { get; set; }

        public bool AutoDelay { get; set; }

        public bool OnlyWindowScreen { get; set; }

        public int Delay { get; set; }

        public string ServerPassword { get; set; }

        public static void SerializeJSON(SVServerSettings ServerSettings, string FName) {

            File.WriteAllText(FName, new JavaScriptSerializer().Serialize(ServerSettings));

        }//SerializeJSON

        public static SVServerSettings DeserializeJSON(string FName) {

            return new JavaScriptSerializer().Deserialize<SVServerSettings>(File.ReadAllText(FName));

        }//SerializeJSON

    }//SVServerSettings

}//ServerWF
