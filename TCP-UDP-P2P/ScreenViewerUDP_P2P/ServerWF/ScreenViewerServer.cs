using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using ScreenViewer;

namespace ServerWF {

    public enum ScreenShotMethod {

        GDI = 0,
        DirectX11 = 1,

    }//ScreenShotMethod

    class ScreenViewerServer : IDisposable {

        int Width_X = SystemInformation.VirtualScreen.Width;

        int Height_Y = SystemInformation.VirtualScreen.Height;

        ScreenShotMethod method;

        bool EnableAudio;

        int TCP_Port;

        int UDP_Video_Port;

        int UDP_Audio_Port;

        string Password;

        IPAddress ip;

        TcpListener TCP;

        UdpClient UDP_Video;

        UdpClient UDP_Audio;

        List<ClientInfo> Clients;

        ScreenShot screenViewer;

        Audio voice;

        bool ServerStart;

        int DeviceNumber;

        int Sleep;

        int fps = 0;

        bool fpsRun = false;

        const int MessageBufferSIZE = 4096;

        byte[] MessageBuffer = new byte[MessageBufferSIZE];

        bool OnlyWindowScreen;

        int FileSendTCP_Port;

        TcpListener FileSendTCP;

        public ScreenViewerServer(int FileSendTCP_Port, bool OnlyWindowScreen, string Password, int Sleep, ScreenShotMethod method, IPAddress ip, int TCP_Port, int UDP_Video_Port, int UDP_Audio_Port = -1, int DeviceNumber = -1) {

            try {

                this.FileSendTCP_Port = FileSendTCP_Port;

                this.OnlyWindowScreen = OnlyWindowScreen;

                this.ServerStart = true;

                this.Password = Password;

                this.Sleep = Sleep;

                this.Clients = new List<ClientInfo>();

                this.method = method;

                this.ip = ip;

                this.TCP_Port = TCP_Port;

                this.UDP_Video_Port = UDP_Video_Port;

                this.TCP = new TcpListener(this.ip, this.TCP_Port);

                this.FileSendTCP = new TcpListener(this.ip, this.FileSendTCP_Port);

                this.UDP_Video = new UdpClient(new IPEndPoint(this.ip, this.UDP_Video_Port));

                if(UDP_Audio_Port == -1) {

                    this.EnableAudio = false;

                }//if
                else {

                    this.EnableAudio = true;

                    this.UDP_Audio_Port = UDP_Audio_Port;

                    this.UDP_Audio = new UdpClient(new IPEndPoint(this.ip, this.UDP_Audio_Port));

                    this.DeviceNumber = DeviceNumber;

                }//else

            }//try
            catch(Exception ex) {

                throw ex;

            }//catch

        }//ScreenViewerServer

        public void Start() {

            this.FileSendTCP.Start();

            Program.SetTitle(Program.GetFirstTitle() + $" | Online {this.Clients.Count}");

            Task TaskTCP = new Task(this.TCPListen);

            TaskTCP.Start();

            Program.ConsoleWriteLine($"Server Started at {this.TCP.LocalEndpoint} ({DateTime.Now.ToLongTimeString()})", Program.Green, Color.Black);

            if (this.method == ScreenShotMethod.GDI) {

                this.screenViewer = new GDIScreenShot(this.OnlyWindowScreen);

                Program.ConsoleWriteLine("Video Screen Method : GDI", Program.Yellow, Color.Black);

            }//if
            else {

                this.screenViewer = new DirectX11ScreenShot();

                Program.ConsoleWriteLine("Video Screen Method : DirectX11", Program.Yellow, Color.Black);

            }//else

            this.screenViewer.ScreenRefreshed += this.SendVideo;

            this.screenViewer.Start();

            Program.ConsoleWriteLine($"Video Stream Port : {this.UDP_Video_Port}", Program.Magenta, Color.Black);

            if (this.EnableAudio) {

                this.voice = new Audio(this.DeviceNumber);

                this.voice.VoiceRefreshed += this.SendAudio;

                this.voice.Start();

                Program.ConsoleWriteLine($"Audio Stream Port : {this.UDP_Audio_Port}", Program.Magenta, Color.Black);

            }//if

            if(this.Sleep == -1) {

                this.Sleep = 0;

                this.fpsRun = true;

                Task.Run(() => {

                    while (this.fpsRun) {

                        Thread.Sleep(1000);

                        //Program.ConsoleWriteLine($"{this.fps}", Program.Yellow, Color.Black);

                        if (this.fps > 30)
                            this.Sleep++;
                        else {

                            if (this.Sleep > 0)
                                this.Sleep--;

                        }//else

                        this.fps = 0;

                    }//while

                });

            }//if

        }//Main

        void TCPListen() {

            try {

                TCP.Start();

                while (true) {

                    TcpClient Client = TCP.AcceptTcpClient();

                    //Program.ConsoleWriteLine(Client.Client.RemoteEndPoint.ToString() + " Connecting", Color.GreenYellow, Color.Black);

                    Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    var ClientStream = Client.GetStream();

                    if (this.EnableAudio == false) {

                        SCMessage message = new SCMessage { MessageType = SCMessageType.Send_Client_Connection_Info, UDP_Video_Port = 1 };

                        var bytes = SCMessage.SerializeJSON_ToBytes(message);

                        ClientStream.Write(bytes, 0, bytes.Length);

                    }//if
                    else {

                        SCMessage message = new SCMessage { MessageType = SCMessageType.Send_Client_Connection_Info, UDP_Video_Port = 1, UDP_Audio_Port = 1 };

                        var bytes = SCMessage.SerializeJSON_ToBytes(message);

                        ClientStream.Write(bytes, 0, bytes.Length);

                    }//else

                    int BytesRead = 0;

                    try {

                        BytesRead = ClientStream.Read(this.MessageBuffer, 0, this.MessageBuffer.Length);

                        var message = SCMessage.DeserializeJSON_FromBytes(this.MessageBuffer, 0, BytesRead);

                        if (message.MessageType == SCMessageType.Send_Client_Connection_Info) {

                            string Password = message.Message;

                            if (this.Password != Password) {

                                SCMessage scmessage = new SCMessage { MessageType = SCMessageType.Invalid_Password };

                                var bytes = SCMessage.SerializeJSON_ToBytes(scmessage);

                                ClientStream.Write(bytes, 0, bytes.Length);

                                Program.ConsoleWriteLine(Client.Client.RemoteEndPoint.ToString() + " (Invalid Password)", Color.Red, Color.Black);

                                Client.Close();

                                continue;

                            }//if

                            string NickName = message.From;

                            var checkNick = this.Clients.FirstOrDefault(c => c.NickName == NickName);

                            if (checkNick != null) {

                                SCMessage scmessage = new SCMessage { MessageType = SCMessageType.Invalid_NickName };

                                var bytes = SCMessage.SerializeJSON_ToBytes(scmessage);

                                ClientStream.Write(bytes, 0, bytes.Length);

                                Program.ConsoleWriteLine(Client.Client.RemoteEndPoint.ToString() + " (Invalid NickName)", Color.Red, Color.Black);

                                Client.Close();

                                continue;

                            }//if

                            SCMessage scmessageOK = new SCMessage { MessageType = SCMessageType.Connection_OK, X = Width_X, Y = Height_Y };

                            var bytesOK = SCMessage.SerializeJSON_ToBytes(scmessageOK);

                            ClientStream.Write(bytesOK, 0, bytesOK.Length);

                            BytesRead = ClientStream.Read(this.MessageBuffer, 0, this.MessageBuffer.Length);

                            scmessageOK = SCMessage.DeserializeJSON_FromBytes(this.MessageBuffer, 0, BytesRead);

                            if (scmessageOK.MessageType == SCMessageType.Connection_OK) {

                                Program.ConsoleWriteLine(Client.Client.RemoteEndPoint.ToString() + $" Connected ({NickName})", Color.GreenYellow, Color.Black);

                                int Video_Udp_Port = message.UDP_Video_Port;

                                int Audio_Udp_Port = -1;

                                if (this.EnableAudio) {

                                    Audio_Udp_Port = message.UDP_Audio_Port;

                                }//if

                                this.SendAll_Clients(SCMessage.SerializeJSON_ToBytes(new SCMessage { Message = NickName, MessageType = SCMessageType.ClientConnected_Message }));

                                AddClientInfo(Client, Video_Udp_Port, Audio_Udp_Port, NickName);

                            }//if
                            else
                                continue;

                        }//if
                        else {

                            Disconnect(Client);

                            continue;

                        }//else

                    }//try
                    catch {

                        Disconnect(Client);

                        continue;

                    }//catch

                    Task.Run(() => {

                        try {

                            this.SendAllClientsOnline(Client);

                            int countRead = 0;

                            SCMessage scmessage = null;

                            byte[] clientMessage = new byte[MessageBufferSIZE];

                            while (true) {

                                countRead = ClientStream.Read(clientMessage, 0, clientMessage.Length);

                                scmessage = SCMessage.DeserializeJSON_FromBytes(clientMessage, 0, countRead);

                                if(scmessage.MessageType == SCMessageType.Mouse_Click) {

                                    MouseClick mouseclick = new MouseClick();

                                    mouseclick.Message = scmessage.From;

                                    mouseclick.X = scmessage.X;

                                    mouseclick.Y = scmessage.Y;

                                    MouseClick.DrawMouseClick(mouseclick);

                                }//if
                                else if (scmessage.MessageType == SCMessageType.PublicMessage) {

                                    Program.ConsoleWriteLine($"{scmessage.From} : {scmessage.Message}", Program.Orange, Color.Black);

                                    var sender = this.Clients.FirstOrDefault(c => c.NickName == scmessage.From);

                                    if (sender != null)
                                        this.SendAll_Clients(clientMessage, 0, countRead, sender);

                                }//else if
                                else if(scmessage.MessageType == SCMessageType.PrivateMessage) {

                                    var toClient = this.Clients.FirstOrDefault(c => c.NickName == scmessage.To);

                                    if (toClient != null)
                                        this.SendTCP(clientMessage, 0, countRead, toClient.Client);

                                }//else if
                                else if(scmessage.MessageType == SCMessageType.DeleteFileServer) {

                                    var fname = scmessage.Message;

                                    var NickName_ = scmessage.From;

                                    Task.Run(() => {

                                        while (true) {

                                            try {

                                                if (File.Exists($"{Program.SV_Directory}/{fname}")) {

                                                    File.Delete($"{Program.SV_Directory}/{fname}");

                                                    return;

                                                }//if

                                            }//try
                                            catch {

                                            }//catch

                                            Thread.Sleep(1000);

                                        }//while

                                    });

                                    scmessage = new SCMessage { MessageType = SCMessageType.FileDeleted, Message = fname };

                                    var bytes_ = SCMessage.SerializeJSON_ToBytes(scmessage);

                                    var sender = this.Clients.FirstOrDefault(c => c.NickName == NickName_);

                                    if (sender != null)
                                        this.SendAll_Clients(bytes_, 0, bytes_.Length, sender);


                                }//else if
                                else if(scmessage.MessageType == SCMessageType.SendFile) {

                                    string NickName_ = scmessage.From;

                                    SCMessage cMessage = new SCMessage { MessageType = SCMessageType.FileTCP_Port, UDP_Video_Port = this.FileSendTCP_Port };

                                    var bytes__ = SCMessage.SerializeJSON_ToBytes(cMessage);

                                    ClientStream.Write(bytes__, 0, bytes__.Length);

                                    Task.Run(() => {

                                        TcpClient client = null;

                                        try {

                                            if (!Directory.Exists(Program.SV_Directory)) {

                                                Directory.CreateDirectory(Program.SV_Directory);

                                            }//if

                                            client = this.FileSendTCP.AcceptTcpClient();

                                            var NS = client.GetStream();

                                            var fname = $"{DateTime.Now.Ticks}_{scmessage.Message}";

                                            FileStream fileStream = new FileStream($"{Program.SV_Directory}/{fname}", FileMode.Create, FileAccess.Write);

                                            const long SendSize = 999;

                                            byte[] buffer = new byte[SendSize];

                                            byte[] messagebyffer = new byte[4] { 7, 7, 7, 7 };

                                            NS.Write(messagebyffer, 0, messagebyffer.Length);

                                            var fnameBytes = Encoding.UTF8.GetBytes(fname);

                                            NS.Write(fnameBytes, 0, fnameBytes.Length);

                                            while (true) {

                                                countRead = NS.Read(buffer, 0, buffer.Length);

                                                if (countRead == 9) {

                                                    bool error = true;

                                                    for (int j = 0; j < 9; j++) {

                                                        if (buffer[j] != 7) {

                                                            error = false;

                                                            return;

                                                        }//if

                                                    }//for

                                                    if (error)
                                                        break;

                                                    fileStream.Write(buffer, 0, countRead);

                                                    NS.Write(messagebyffer, 0, messagebyffer.Length);

                                                    fileStream.Close();

                                                    break;

                                                }//if
                                                else if (countRead < SendSize) {

                                                    fileStream.Write(buffer, 0, countRead);

                                                    NS.Write(messagebyffer, 0, messagebyffer.Length);

                                                    fileStream.Close();

                                                    break;

                                                }//else if
                                                else {

                                                    fileStream.Write(buffer, 0, countRead);

                                                    NS.Write(messagebyffer, 0, messagebyffer.Length);

                                                }//else

                                            }//while

                                            SCMessage sCMessage = new SCMessage { MessageType = SCMessageType.SendFileName, Message = fname };

                                            var bytes___ = SCMessage.SerializeJSON_ToBytes(sCMessage);

                                            var sender = this.Clients.FirstOrDefault(c => c.NickName == NickName_);

                                            if (sender != null)
                                                this.SendAll_Clients(bytes___, 0, bytes___.Length, sender);

                                        }//try
                                        catch (Exception ex) {

                                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                        }//catch

                                        if (client != null)
                                            client.Close();

                                    });

                                }//else if
                                else if(scmessage.MessageType == SCMessageType.SendFileName) {

                                    if (File.Exists($"{Program.SV_Directory}/{scmessage.Message}") == false) {

                                        scmessage = new SCMessage { MessageType = SCMessageType.FileNotFound, Message = scmessage.Message };

                                        var bytes = SCMessage.SerializeJSON_ToBytes(scmessage);

                                        ClientStream.Write(bytes, 0, bytes.Length);

                                        return;

                                    }//if
                                    else {

                                        SCMessage cMessage = new SCMessage { MessageType = SCMessageType.SendFile, UDP_Video_Port = this.FileSendTCP_Port };

                                        var bytes = SCMessage.SerializeJSON_ToBytes(cMessage);

                                        ClientStream.Write(bytes, 0, bytes.Length);

                                    }//else

                                    Task.Run(() => {

                                        TcpClient client = null;

                                        try {

                                            client = this.FileSendTCP.AcceptTcpClient();

                                            var NS = client.GetStream();

                                            var fname = scmessage.Message;

                                            try {

                                                const long SendSize = 999;

                                                byte[] read = new byte[4];

                                                NS.Read(read, 0, read.Length);

                                                FileStream fileStream = new FileStream($"{Program.SV_Directory}/{fname}", FileMode.Open, FileAccess.Read);

                                                FileInfo fileInfo = new FileInfo($"{Program.SV_Directory}/{fname}");

                                                var countFullSend = (int)(fileInfo.Length / SendSize);

                                                var lastBytes = (int)(fileInfo.Length - (countFullSend * SendSize));

                                                int i = 0;

                                                byte[] buffer = new byte[SendSize];

                                                while (i < countFullSend) {

                                                    fileStream.Read(buffer, 0, buffer.Length);

                                                    NS.Write(buffer, 0, buffer.Length);

                                                    NS.Read(read, 0, read.Length);

                                                    i++;

                                                }//while

                                                if (lastBytes == 0) {

                                                    ClientStream.Write(new byte[9] { 7, 7, 7, 7, 7, 7, 7, 7, 7 }, 0, 9);

                                                }//if
                                                else {

                                                    fileStream.Read(buffer, 0, lastBytes);

                                                    NS.Write(buffer, 0, lastBytes);

                                                    NS.Read(read, 0, read.Length);

                                                    fileStream.Close();

                                                }//else

                                            }//try
                                            catch (Exception ex) {

                                                ClientStream.Write(new byte[9] { 7, 7, 7, 7, 7, 7, 7, 7, 7 }, 0, 9);

                                                MessageBox.Show(ex.Message, "Send File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                            }//catch

                                        }//try
                                        catch (Exception ex) {

                                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                        }//catch

                                        if (client != null)
                                            client.Close();

                                    });

                                }//else if

                            }//while


                        }//try
                        catch {

                            Disconnect(Client);

                            return;

                        }//catch

                    });

                }//while

            }//try
            catch (Exception ex) {

                //Program.ConsoleWriteLine($"TCP Error: {ex.Message}", Program.Red, Color.Black);

            }//catch

        }//TCPListen

        public void SendFileServer(string Fname) {

            SCMessage message = new SCMessage { MessageType = SCMessageType.SendFileName, Message = Fname };

            var bytes = SCMessage.SerializeJSON_ToBytes(message);

            this.SendAll_Clients(bytes, 0, bytes.Length);

        }//SendFileServer

        public void SendServerMessage(string Text) {

            SCMessage message = new SCMessage { MessageType = SCMessageType.PublicMessage, From = "Server Admin", Message = Text };

            var bytes = SCMessage.SerializeJSON_ToBytes(message);

            this.SendAll_Clients(bytes, 0 , bytes.Length);

        }//SendServerMessage

        void SendAllClientsOnline(TcpClient client) {

            SCMessage message = new SCMessage { MessageType = SCMessageType.All_Clients_Online };

            string messageText = string.Empty;
            
            for(int i = 0; i < this.Clients.Count; i++) {

                if (this.Clients[i].Client == client)
                    continue;

                if(i + 1 == this.Clients.Count) {

                    messageText += this.Clients[i].NickName;

                    break;

                }//if

                messageText += this.Clients[i].NickName + " ";

            }//for

            message.Message = messageText;

            var bytes = SCMessage.SerializeJSON_ToBytes(message);

            client.GetStream().Write(bytes, 0, bytes.Length);

        }//SendAllClientsOnline

        void AddClientInfo(TcpClient client, int Video_Udp_Port, int Audio_Udp_Port, string NickName) {

            var clientInfo = new ClientInfo();

            clientInfo.NickName = NickName;

            var ip = (client.Client.RemoteEndPoint as IPEndPoint).Address;

            string firestIpbytes = ip.ToString().Substring(0, ip.ToString().IndexOf('.'));

            if (firestIpbytes == "192" || firestIpbytes == "172" || firestIpbytes == "10") {

                clientInfo.ConnectionType = ConnectionTypes.LAN;

            }//if
            else {

                clientInfo.ConnectionType = ConnectionTypes.WAN;

            }//else

            clientInfo.VideoEndpoint = new IPEndPoint(ip, Video_Udp_Port);

            if (this.EnableAudio) {

                clientInfo.AudioEndpoint = new IPEndPoint(ip, Audio_Udp_Port);

            }//if
            else {

                clientInfo.AudioEndpoint = null;

            }//else

            clientInfo.Client = client;

            if (Clients.Count > 0) {

                var lastClient = Clients[Clients.Count - 1];

                if (clientInfo.ConnectionType == ConnectionTypes.LAN && lastClient.ConnectionType == ConnectionTypes.WAN) {

                    if (Clients.Count == 1) {

                        //SendTCP(Encoding.ASCII.GetBytes($"{Clients[0].VideoEndpoint.ToString()};{(this.EnableAudio ? Clients[0].AudioEndpoint.ToString() : string.Empty)}"), clientInfo.Client);

                        SCMessage message = new SCMessage { IP = Clients[0].VideoEndpoint.Address.ToString(), UDP_Video_Port = Clients[0].VideoEndpoint.Port, UDP_Audio_Port = this.EnableAudio ? Clients[0].AudioEndpoint.Port : 0, MessageType = SCMessageType.Send_Client_IP_Ports_Info };

                        SendTCP(SCMessage.SerializeJSON_ToBytes(message), clientInfo.Client);

                        Clients.Insert(0, clientInfo);

                    }//if
                    else {

                        int FirstWANIndex = Clients.IndexOf(Clients.First(c => c.ConnectionType == ConnectionTypes.WAN));

                        //SendTCP(Encoding.ASCII.GetBytes($"{clientInfo.VideoEndpoint.ToString()};{(this.EnableAudio ? clientInfo.AudioEndpoint.ToString() : string.Empty)}"), Clients[FirstWANIndex - 1].Client);

                        SCMessage message = new SCMessage { IP = clientInfo.VideoEndpoint.Address.ToString(), UDP_Video_Port = clientInfo.VideoEndpoint.Port, UDP_Audio_Port = this.EnableAudio ? clientInfo.AudioEndpoint.Port : 0, MessageType = SCMessageType.Send_Client_IP_Ports_Info };

                        SendTCP(SCMessage.SerializeJSON_ToBytes(message), Clients[FirstWANIndex - 1].Client);

                        //SendTCP(Encoding.ASCII.GetBytes($"{Clients[FirstWANIndex].VideoEndpoint.ToString()};{(this.EnableAudio ? Clients[FirstWANIndex].AudioEndpoint.ToString() : string.Empty)}"), clientInfo.Client);

                        message = new SCMessage { IP = Clients[FirstWANIndex].VideoEndpoint.Address.ToString(), UDP_Video_Port = Clients[FirstWANIndex].VideoEndpoint.Port, UDP_Audio_Port = this.EnableAudio ? Clients[FirstWANIndex].AudioEndpoint.Port : 0, MessageType = SCMessageType.Send_Client_IP_Ports_Info };

                        SendTCP(SCMessage.SerializeJSON_ToBytes(message), clientInfo.Client);

                        Clients.Insert(FirstWANIndex, clientInfo);

                    }//else

                }//if
                else {

                    Clients.Add(clientInfo);

                    //SendTCP(Encoding.ASCII.GetBytes($"{clientInfo.VideoEndpoint.ToString()};{(this.EnableAudio ? clientInfo.AudioEndpoint.ToString() : string.Empty)}"), Clients[Clients.Count - 2].Client);

                    SCMessage message = new SCMessage { IP = clientInfo.VideoEndpoint.Address.ToString(), UDP_Video_Port = clientInfo.VideoEndpoint.Port, UDP_Audio_Port = this.EnableAudio ? clientInfo.AudioEndpoint.Port : 0, MessageType = SCMessageType.Send_Client_IP_Ports_Info };

                    SendTCP(SCMessage.SerializeJSON_ToBytes(message), Clients[Clients.Count - 2].Client);

                }//else

            }//if
            else {

                Clients.Add(clientInfo);

            }//else

            Program.SetTitle(Program.GetFirstTitle() + $" | Online {this.Clients.Count}");

        }//AddClientInfo

        void Disconnect(TcpClient Client) {

            try {

                var CI = Clients.FirstOrDefault(c => c.Client == Client);

                string nickName = string.Empty;

                if (CI != null) {

                    nickName = CI.NickName.Clone() as string;

                    this.SendAll_Clients(SCMessage.SerializeJSON_ToBytes(new SCMessage { Message = nickName, MessageType = SCMessageType.ClientDisconnected_Message }), CI);

                    int index = Clients.IndexOf(CI);

                    if (Clients.Count == index + 1 && index != 0) {

                        SCMessage message = new SCMessage { MessageType = SCMessageType.Client_Disconnected };

                        SendTCP(SCMessage.SerializeJSON_ToBytes(message), Clients[index - 1].Client);

                    }//if
                    else if (Clients.Count >= 3 && index >= 1) {

                        //SendTCP(Encoding.ASCII.GetBytes($"{Clients[index + 1].VideoEndpoint.ToString()};{(this.EnableAudio ? Clients[index + 1].AudioEndpoint.ToString() : string.Empty)}"), Clients[index - 1].Client);

                        SCMessage message = new SCMessage { IP = Clients[index + 1].VideoEndpoint.Address.ToString(), UDP_Video_Port = Clients[index + 1].VideoEndpoint.Port, UDP_Audio_Port = this.EnableAudio ? Clients[index + 1].AudioEndpoint.Port : 0, MessageType = SCMessageType.Send_Client_IP_Ports_Info };

                        SendTCP(SCMessage.SerializeJSON_ToBytes(message), Clients[index - 1].Client);

                    }//else if

                    Clients.Remove(CI);

                }//if

                Program.ConsoleWriteLine(Client.Client.RemoteEndPoint.ToString() + $" Disconnected ({nickName})", Color.OrangeRed, Color.Black);

                Client.Close();

            }//try
            catch {

            }//catch

            Program.SetTitle(Program.GetFirstTitle() + $" | Online {this.Clients.Count}");

        }//Disconnect

        void SendAll_Clients(byte[] data) {

            this.SendAll_Clients(data, 0, data.Length);

        }//SendAll_Clients

        void SendAll_Clients(byte[] data, ClientInfo InAdditionTo) {

            this.SendAll_Clients(data, 0, data.Length, InAdditionTo);

        }//SendAll_Clients

        void SendAll_Clients(byte[] data, int offset, int size) {

            foreach (var client in this.Clients) {

                client.Client.GetStream().Write(data, offset, size);

            }//foreach

        }//SendAll_Clients

        void SendAll_Clients(byte[] data, int offset, int size, ClientInfo InAdditionTo) {

            foreach (var client in this.Clients) {

                if (client == InAdditionTo)
                    continue;

                client.Client.GetStream().Write(data, offset, size);

            }//foreach

        }//SendAll_Clients

        void SendTCP(byte[] data, TcpClient Client) {

            this.SendTCP(data, 0, data.Length, Client);

        }//SendTCP

        void SendTCP(byte[] data, int offset, int size, TcpClient Client) {

            if (Client != null && Client.Connected) {

                NetworkStream NetStream = Client.GetStream();
                NetStream.Write(data, offset, size);

            }//if

        }//SendTCP

        public void Dispose() {

            this.ServerStart = false;

            Program.ConsoleWriteLine($"Server Stop ({DateTime.Now.ToLongTimeString()})", Program.Red, Color.Black);

            SCMessage message = new SCMessage { MessageType = SCMessageType.Server_Exit };

            for (int i = 0; i < this.Clients.Count; i++) {

                message.Message = i.ToString();

                var ExitMessage = SCMessage.SerializeJSON_ToBytes(message);

                this.Clients[i].Client.GetStream().Write(ExitMessage, 0, ExitMessage.Length);

                this.Clients[i].Client.Close();

                this.Clients[i].Client = null;

            }//for

            this.FileSendTCP.Stop();

            this.FileSendTCP = null;

            this.Clients = null;

            this.fpsRun = false;

            this.TCP.Stop();

            this.TCP = null;

            this.UDP_Video.Close();

            this.UDP_Video = null;

            this.screenViewer.ScreenRefreshed = null;

            this.screenViewer.Stop();

            this.screenViewer = null;

            if (this.EnableAudio) {

                this.UDP_Audio.Close();

                this.UDP_Audio = null;

                this.voice.VoiceRefreshed = null;

                this.voice.Stop();

                this.voice = null;

            }//if

        }//Dispose

        void SendVideo(object sender, byte[][] data) {

            if (this.Clients.Count > 0) {

                try {

                    Thread.Sleep(this.Sleep);

                    byte Count = (byte)data.Length;

                    this.UDP_Video.Send(new byte[1] { Count }, 1, this.Clients[0].VideoEndpoint);

                    Thread.Sleep(1);

                    foreach (var item in data) {

                        this.UDP_Video.Send(item, item.Length, this.Clients[0].VideoEndpoint);

                        for (int i = 0; i < 2000; i++)
                            Thread.Sleep(0);

                    }//foreach

                    if (this.fpsRun)
                        this.fps++;

                }//try
                catch {


                }//catch

            }//if
            else
                Thread.Sleep(200);

        }//SendVideo

        void SendAudio(object sender, byte[] data) {

            if (this.Clients.Count > 0) {

                try {

                    this.UDP_Audio.Send(data, data.Length, this.Clients[0].AudioEndpoint);

                }//try
                catch {


                }//catch

            }//if
            else
                Thread.Sleep(200);

        }//SendAudio

    }//ScreenViewerServer

}//ServerWF
