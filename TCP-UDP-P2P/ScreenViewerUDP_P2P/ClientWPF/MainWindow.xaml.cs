using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using ScreenViewer;

namespace ClientWPF {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        IPEndPoint TCPEndPoint;
        public TcpClient TCP;

        IPEndPoint UDPEndPoint;

        UdpClient UDP_Video;
        UdpClient UDP_Audio;

        IPEndPoint nextClientVideo;

        IPEndPoint nextClientAudio;

        bool ShowBrokenImage;

        public ConnectionSettings connectionSettings;

        const int MessageBufferSIZE = 4096;

        byte[] MessageBuffer = new byte[MessageBufferSIZE];

        float ServerX = 0;

        float ServerY = 0;

        DoubleAnimation angleDA;

        double MenuLabelWidth;

        bool ChatClick = false;

        bool SendFile = false;

        public bool DownloadFile = false;

        string SendFileName;

        public string DownloadFilePath;

        public StackPanel _ChatStackPanel { get => this.Chat_StackPanel; }

        public MainWindow() {

            InitializeComponent();

            this.Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ClientWPF.Properties.Resources.screen.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            this.To_Message_ComboBox.Items.Add("Public");

            this.To_Message_ComboBox.SelectedIndex = 0;

            this.MenuLabelWidth = this.MenuTextBlock.Width;
            this.angleDA = new DoubleAnimation();
            this.angleDA.Duration = TimeSpan.FromSeconds(0.3);
            this.angleDA.DecelerationRatio = 0.8;

        }//MainWindow

        private void Window_Loaded(object sender, RoutedEventArgs e) {

            ConnectionWindow connection = new ConnectionWindow();

            if (connection.ShowDialog().Value) {

                this.connectionSettings = connection.connectionSettings;

                Task.Run(() => {

                    try {

                        this.Connection();

                    }//try
                    catch (Exception ex) {

                        MessageBox.Show(ex.Message);

                        this.Dispatcher.Invoke(() => {

                            Application.Current.Shutdown();

                        });

                        return;

                    }//catch

                    this.Dispatcher.Invoke(() => {

                        this.WindowState = WindowState.Maximized;

                    });

                    this.StartThreads();

                });

            }//if
            else {

                Application.Current.Shutdown();

            }//else

        }//Window_Loaded

        void Connection() {

            TCP = new TcpClient();

            this.ShowBrokenImage = this.connectionSettings.ShowBrokenImage;

            this.TCPEndPoint = new IPEndPoint(IPAddress.Parse(this.connectionSettings.IP), this.connectionSettings.Port);

            this.UDPEndPoint = new IPEndPoint(IPAddress.Parse(this.connectionSettings.IP), this.connectionSettings.Port);

            if (this.connectionSettings.RandomSend)
                this.UDP_Video = new UdpClient();
            else
                this.UDP_Video = new UdpClient(this.connectionSettings.VideoSendPort);

            this.UDP_Video.Send(new byte[1], 1, UDPEndPoint);

            this.UDP_Video.AllowNatTraversal(true);

            this.UDP_Video.Client.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);

            this.UDP_Video.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            this.TCP.Connect(TCPEndPoint);

            int count = TCP.GetStream().Read(this.MessageBuffer, 0, this.MessageBuffer.Length);

            var message = SCMessage.DeserializeJSON_FromBytes(this.MessageBuffer, 0, count);

            if (message.MessageType == SCMessageType.Send_Client_Connection_Info && message.UDP_Audio_Port == 0) {

                SCMessage video_port = new SCMessage { UDP_Video_Port = (UDP_Video.Client.LocalEndPoint as IPEndPoint).Port, MessageType = SCMessageType.Send_Client_Connection_Info, From = this.connectionSettings.NickName, Message = this.connectionSettings.ServerPassword };

                var bytes = SCMessage.SerializeJSON_ToBytes(video_port);

                this.TCP.GetStream().Write(bytes, 0, bytes.Length);

            }//if
            else if(message.MessageType == SCMessageType.Send_Client_Connection_Info && message.UDP_Audio_Port == 1) {

                if (this.connectionSettings.RandomSend)
                    this.UDP_Audio = new UdpClient();
                else                   
                    this.UDP_Audio = new UdpClient(this.connectionSettings.AudioSendPort);

                this.UDP_Audio.Send(new byte[1], 1, UDPEndPoint);

                this.UDP_Audio.AllowNatTraversal(true);

                this.UDP_Audio.Client.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);

                this.UDP_Audio.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                SCMessage video_audio_port = new SCMessage { UDP_Video_Port = (this.UDP_Video.Client.LocalEndPoint as IPEndPoint).Port, UDP_Audio_Port = (this.UDP_Audio.Client.LocalEndPoint as IPEndPoint).Port, MessageType = SCMessageType.Send_Client_Connection_Info, From = this.connectionSettings.NickName, Message = this.connectionSettings.ServerPassword };

                var bytes = SCMessage.SerializeJSON_ToBytes(video_audio_port);

                this.TCP.GetStream().Write(bytes, 0, bytes.Length);

            }//else

            count = TCP.GetStream().Read(this.MessageBuffer, 0, this.MessageBuffer.Length);

            message = SCMessage.DeserializeJSON_FromBytes(this.MessageBuffer, 0, count);

            if(message.MessageType == SCMessageType.Invalid_Password) {

                MessageBox.Show("Invalid Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                this.Dispatcher.Invoke(() => {

                    Application.Current.Shutdown();

                });

            }//if
            else if(message.MessageType == SCMessageType.Invalid_NickName) {

                MessageBox.Show("Invalid NickName, Please Change Your NickName", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                this.Dispatcher.Invoke(() => {

                    Application.Current.Shutdown();

                });

            }//else if         
            else if(message.MessageType == SCMessageType.Connection_OK) {

                this.ServerX = message.X;

                this.ServerY = message.Y;

                SCMessage connection_OK = new SCMessage { MessageType = SCMessageType.Connection_OK };

                var bytes = SCMessage.SerializeJSON_ToBytes(connection_OK);

                this.TCP.GetStream().Write(bytes, 0, bytes.Length);

            }//else if
            else {

                throw new Exception("SERVER ERROR MESSAGE");

            }//else

        }//Connection

        void CleanConnection() {

            this.Dispatcher.Invoke(() => {

                this.ChatStackPanel.AllowDrop = false;
                this.Send_Message_Button.IsEnabled = false;
                this.Chat_StackPanel.IsEnabled = false;

            });

            this.TCPEndPoint = null;
            this.TCP.Dispose();
            this.TCP = null;
            this.UDPEndPoint= null;
            this.UDP_Video.Dispose();
            this.UDP_Video = null;

            if(this.UDP_Audio != null) {

                this.UDP_Audio.Dispose();
                this.UDP_Audio = null;

            }//if

            this.nextClientVideo = null;
            this.nextClientAudio = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }//CleanConnection

        void StartThreads() {

            ThreadPool.QueueUserWorkItem(this.TCP_Message);

            Thread videoTread = new Thread(this.GetVideo);

            videoTread.IsBackground = true;

            videoTread.Priority = ThreadPriority.Highest;

            videoTread.Start();

            if (this.UDP_Audio != null)
                ThreadPool.QueueUserWorkItem(this.GetAudio);

        }//StartThreads

        void PacketsToImage(byte[][] data) {

            try {

                MemoryStream ms = new MemoryStream();

                for(int i = 0; i < data.Length; i++) {

                    if (data[i].Length != 1)
                        ms.Write(data[i], 0, data[i].Length);

                }//for

                ms.Seek(0, SeekOrigin.Begin);

                BitmapImage biImg = new BitmapImage();

                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                this.MainImage.Source = biImg;

            }//try
            catch {


            }//catch

        }//MSToImage

        void TCP_Message(object obj) {

            int ConnectionSleep = 0;

            try {

                int readBytes = 0;

                NetworkStream ns = TCP.GetStream();

                SCMessage message = null;

                while (true) {

                    readBytes = ns.Read(this.MessageBuffer, 0, this.MessageBuffer.Length);

                    try{

                        message = SCMessage.DeserializeJSON_FromBytes(this.MessageBuffer, 0, readBytes);

                        if (message == null)
                            continue;

                    }//try
                    catch {

                        continue;

                    }//catch

                    if(message.MessageType == SCMessageType.Send_Client_IP_Ports_Info) {

                        this.nextClientVideo = new IPEndPoint(IPAddress.Parse(message.IP), message.UDP_Video_Port);

                        if(message.UDP_Audio_Port != 0)
                            this.nextClientAudio = new IPEndPoint(IPAddress.Parse(message.IP), message.UDP_Video_Port);

                    }//if
                    else if(message.MessageType == SCMessageType.Server_Exit) {

                        ConnectionSleep = int.Parse(message.Message);

                        break;

                    }//else if
                    else if(message.MessageType == SCMessageType.Client_Disconnected) {

                        this.nextClientVideo = null;

                        this.nextClientAudio = null;

                    }//else if
                    else if(message.MessageType == SCMessageType.ClientConnected_Message) {

                        this.Dispatcher.Invoke(() => {

                            this.To_Message_ComboBox.Items.Add(message.Message);

                        });

                    }//else if
                    else if(message.MessageType == SCMessageType.ClientDisconnected_Message) {

                        if (this.To_Message_ComboBox.Items.Count >= 2) {

                            for (int i = 1; i < this.To_Message_ComboBox.Items.Count; i++) {

                                if (this.To_Message_ComboBox.Items[i].ToString() == message.Message) {

                                    this.Dispatcher.Invoke(() => {

                                        this.To_Message_ComboBox.SelectedIndex = 0;

                                        this.To_Message_ComboBox.Items.RemoveAt(i);

                                    });

                                    break;

                                }//if

                            }//for

                        }//if                        

                    }//else if
                    else if (message.MessageType == SCMessageType.PublicMessage) {

                        this.Dispatcher.Invoke(() => {

                            this.Chat_StackPanel.Children.Add(new ChatMessage(false, message.Message, message.From));

                        });

                    }//else if
                    else if (message.MessageType == SCMessageType.PrivateMessage) {

                        this.Dispatcher.Invoke(() => {

                            this.Chat_StackPanel.Children.Add(new ChatMessage(false, message.Message, message.From, message.To));

                        });

                    }//else if
                    else if(message.MessageType == SCMessageType.All_Clients_Online) {

                        var NickNames = message.Message.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var NickName in NickNames) {

                            this.Dispatcher.Invoke(() => {

                                this.To_Message_ComboBox.Items.Add(NickName);

                            });

                        }//foreach

                    }//else if
                    else if(message.MessageType == SCMessageType.FileNotFound) {

                        MessageBox.Show($"File Not Found ({message.Message})", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }//else if
                    else if(message.MessageType == SCMessageType.SendFileName) {

                        this.Dispatcher.Invoke(() => {

                            this.Chat_StackPanel.Children.Add(new ChatFileMessage(false, message.Message, this));

                        });

                    }//else if
                    else if(message.MessageType == SCMessageType.FileDeleted) {

                        this.Dispatcher.Invoke(() => {

                            var child = this.Chat_StackPanel.Children.OfType<ChatFileMessage>().FirstOrDefault(c => c.FileName.Text == message.Message);

                            if (child != null)
                                this.Chat_StackPanel.Children.Remove(child);

                        });

                    }//else if
                    else if(message.MessageType == SCMessageType.SendFile) {

                        Task.Run(() => {

                            this.DownloadFile = true;

                            this.DownloadTCPFile(message.UDP_Video_Port);

                            this.DownloadFile = false;

                        });

                    }//else if
                    else if(message.MessageType == SCMessageType.FileTCP_Port) {

                        Task.Run(() => {

                            this.SendTCPFile(message.UDP_Video_Port);

                        });

                    }//else if

                }//while

            }//try
            catch {

                MessageBox.Show("Server crashed");

                this.Dispatcher.Invoke(() => {

                    Application.Current.Shutdown();

                });

                return;

            }//catch

            this.CleanConnection();

            Thread.Sleep(300);

            while (true) {

                try {

                    Thread.Sleep(ConnectionSleep * 300);

                    this.Connection();

                    this.StartThreads();

                    this.Dispatcher.Invoke(() => {

                        this.ChatStackPanel.AllowDrop = true;
                        this.Send_Message_Button.IsEnabled = true;
                        this.Chat_StackPanel.IsEnabled = true;

                    });

                    return;

                }//try
                catch {


                }//catch

            }//while

        }//TCP_Message

        void GetVideo(object obj) {

            //var ipend = this.UDP_Video.Client.LocalEndPoint as IPEndPoint;

            var ipend = new IPEndPoint(IPAddress.Any, 0);

            byte[][] packets = null;

            bool broken = false;

            //int fps = 0;

            //Task.Run(() => {

            //    while (true) {

            //        Thread.Sleep(1000);

            //        this.Dispatcher.Invoke(() => {

            //            this.Title = fps.ToString();

            //        });

            //        fps = 0;

            //    }//while

            //});

            while (true) {

                try {

                    var bytes = this.UDP_Video.Receive(ref ipend);

                    if (bytes.Length == 1) {

                        byte Count = bytes[0];

                        if (this.nextClientVideo != null)
                            this.UDP_Video.Send(new byte[1] { Count }, 1, this.nextClientVideo);

                        packets = new byte[Count][];

                        for (int i = 0; i < packets.Length; i++) {

                            packets[i] = this.UDP_Video.Receive(ref ipend);

                            if (this.nextClientVideo != null)
                                this.UDP_Video.Send(packets[i], packets[i].Length, this.nextClientVideo);

                            if (packets[i].Length == 1) {

                                broken = true;

                                break;

                            }//if

                        }//for

                        if (broken) {

                            //fps++;

                            if (this.ShowBrokenImage) {

                                this.Dispatcher.Invoke(() => {

                                    this.PacketsToImage(packets);

                                });

                            }//if

                        }//if
                        else {

                            //fps++;

                            this.Dispatcher.Invoke(() => {

                                this.PacketsToImage(packets);

                            });

                        }//else

                        broken = false;

                    }//if         

                }//try
                catch (ObjectDisposedException ode) {

                    return;

                }//catch
                catch {

                }//catch

            }//while

        }//GetVideo

        void GetAudio(object obj) {

            BufferedWaveProvider bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));

            WaveOut voice = new WaveOut();

            voice.Init(bufferedWaveProvider);

            voice.Play();

            voice.Volume = 1;

            var ipend = this.UDP_Audio.Client.LocalEndPoint as IPEndPoint;

            while (true) {

                try {

                    var bytes = this.UDP_Audio.Receive(ref ipend);

                    if (this.nextClientAudio != null)
                        this.UDP_Audio.Send(bytes, bytes.Length, this.nextClientAudio);

                    bufferedWaveProvider.AddSamples(bytes, 0, bytes.Length);

                }//try
                catch (ObjectDisposedException ode) {

                    return;

                }//catch
                catch {

                }//catch

            }//while

        }//GetAudio

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e) {

            if (this.WindowState == WindowState.Maximized && this.WindowStyle == WindowStyle.None) {

                this.WindowStyle = WindowStyle.SingleBorderWindow;

                this.WindowState = WindowState.Normal;

            }//if
            else {

                this.WindowStyle = WindowStyle.None;

                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Minimized;

                this.WindowState = WindowState.Maximized;

            }//else

        }//Window_MouseDoubleClick

        private void Window_KeyDown(object sender, KeyEventArgs e) {

            if(e.Key == Key.Escape && this.WindowStyle == WindowStyle.None) {

                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;

            }//if

        }//Window_KeyDown

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {

            if (e.ClickCount != 1)
                return;

            if (this.ChatClick) {

                this.ChatClick = false;

                return;

            }//if

            try {

                var point = e.GetPosition(this.MainImage);

                var X = point.X / this.MainImage.ActualWidth * this.ServerX;

                var Y = point.Y / this.MainImage.ActualHeight * this.ServerY;

                SCMessage message = new SCMessage { MessageType = SCMessageType.Mouse_Click, X = (float)X, Y = (float)Y, From = this.connectionSettings.NickName };

                var bytes = SCMessage.SerializeJSON_ToBytes(message);

                this.TCP.GetStream().Write(bytes, 0, bytes.Length);

            }//try
            catch {


            }//catch

        }//Grid_MouseLeftButtonDown

        private void Send_Message_Button_Click(object sender, RoutedEventArgs e) {

            var messageText = this.Input_Chat_TextBox.Text.Trim();

            if(messageText.Length > 0) {

                SCMessage message = new SCMessage { From = this.connectionSettings.NickName, Message = messageText };

                if (this.To_Message_ComboBox.SelectedIndex == 0) {

                    message.MessageType = SCMessageType.PublicMessage;

                    this.Chat_StackPanel.Children.Add(new ChatMessage(true, message.Message, message.From));

                }//if                
                else {

                    message.MessageType = SCMessageType.PrivateMessage;

                    message.To = this.To_Message_ComboBox.SelectedItem.ToString();

                    this.Chat_StackPanel.Children.Add(new ChatMessage(true, message.Message, message.From, message.To));

                }//else

                var bytes = SCMessage.SerializeJSON_ToBytes(message);

                this.TCP.GetStream().Write(bytes, 0, bytes.Length);

            }//if

            this.Input_Chat_TextBox.Text = string.Empty;

        }//Send_Message_Button_Click

        private void ChatStackPanel_Drop(object sender, DragEventArgs e) {

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (!File.Exists(files[0]))
                    return;

                if (this.SendFile) {

                    MessageBox.Show("Previous File Not Send!", "Wait", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;

                }//if

                this.SendFile = true;

                this.SendFileName = files[0];

                SCMessage message = new SCMessage { MessageType = SCMessageType.SendFile, From = this.connectionSettings.NickName };

                message.Message = System.IO.Path.GetFileName(files[0]);

                var bytes = SCMessage.SerializeJSON_ToBytes(message);

                this.TCP.GetStream().Write(bytes, 0, bytes.Length);

            }//if

        }//ChatStackPanel_Drop

        private void SendTCPFile(int Port) {

            try {

                TcpClient tcpClient = new TcpClient();

                tcpClient.Connect(this.connectionSettings.IP, Port);

                var NS = tcpClient.GetStream();

                const long SendSize = 999;

                byte[] read = new byte[4];

                NS.Read(read, 0, read.Length);

                byte[] fnameBytes = new byte[500];

                int readCount = NS.Read(fnameBytes, 0, fnameBytes.Length);

                var fname = Encoding.UTF8.GetString(fnameBytes, 0, readCount);

                FileStream fileStream = new FileStream(this.SendFileName, FileMode.Open, FileAccess.Read);

                FileInfo fileInfo = new FileInfo(this.SendFileName);

                var countFullSend = (int)(fileInfo.Length / SendSize);

                var lastBytes = (int)(fileInfo.Length - (countFullSend * SendSize));

                int i = 0;

                ChatFileMessage chatFileMessage = null;

                this.Dispatcher.Invoke(() => {

                    chatFileMessage = new ChatFileMessage(true, fname, this);

                    this.Chat_StackPanel.Children.Add(chatFileMessage);

                });

                byte[] buffer = new byte[SendSize];

                while (i < countFullSend) {

                    fileStream.Read(buffer, 0, buffer.Length);

                    NS.Write(buffer, 0, buffer.Length);

                    NS.Read(read, 0, read.Length);

                    i++;

                }//while

                if (lastBytes == 0) {

                    this.TCP.GetStream().Write(new byte[9] { 7, 7, 7, 7, 7, 7, 7, 7, 7 }, 0, 9);

                }//if
                else {

                    fileStream.Read(buffer, 0, lastBytes);

                    NS.Write(buffer, 0, lastBytes);

                    NS.Read(read, 0, read.Length);

                    fileStream.Close();

                }//else

                chatFileMessage.SendOK();

            }//try
            catch (Exception ex) {

                this.TCP.GetStream().Write(new byte[9] { 7, 7, 7, 7, 7, 7, 7, 7, 7 }, 0, 9);

                MessageBox.Show(ex.Message, "Send File Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }//catch

            this.SendFile = false;

        }//SendFile

        private void DownloadTCPFile(int Port) {

            try {

                FileStream fileStream = new FileStream(this.DownloadFilePath, FileMode.Create, FileAccess.Write);

                const long SendSize = 999;

                byte[] buffer = new byte[SendSize];

                byte[] messagebyffer = new byte[4] { 7, 7, 7, 7 };

                TcpClient tcpClient = new TcpClient();

                tcpClient.Connect(this.connectionSettings.IP, Port);

                var NS = tcpClient.GetStream();

                NS.Write(messagebyffer, 0, messagebyffer.Length);

                int countRead = 0;

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

            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }//catch

        }//DownloadTCPFile

        //////////////////////////////////////////////Chat

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e) {

            this.Cursor = Cursors.Hand;

        }//TextBlock_MouseEnter

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e) {

            this.Cursor = Cursors.Arrow;

        }//TextBlock_MouseLeave

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) {

            GridLengthAnimation da = new GridLengthAnimation();
            da.Duration = TimeSpan.FromSeconds(0.3);
            da.DecelerationRatio = 0.3;
            da.AccelerationRatio = 0.7;

            DoubleAnimation daMenu = new DoubleAnimation();
            daMenu.Duration = TimeSpan.FromSeconds(0.3);
            daMenu.DecelerationRatio = 0.3;
            daMenu.AccelerationRatio = 0.7;

            if (this.SideBarColumn.Width.Value == 300) {

                daMenu.From = this.MenuLabelWidth;
                daMenu.To = 0;

                da.From = new GridLength(300);
                da.To = new GridLength(0);

                ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

                thicknessAnimation.From = new Thickness(0);

                thicknessAnimation.To = new Thickness(-33, 0, 0, 0);

                thicknessAnimation.Duration = TimeSpan.FromSeconds(0.3);

                this.ChatButton_TextBlock.BeginAnimation(TextBlock.MarginProperty, thicknessAnimation);

            }//if
            else {

                da.From = new GridLength(0);
                da.To = new GridLength(300);

                daMenu.From = 0;
                daMenu.To = MenuLabelWidth;

            }//else

            this.MenuTextBlock.BeginAnimation(WidthProperty, daMenu);

            this.SideBarColumn.BeginAnimation(ColumnDefinition.WidthProperty, da);

            this.ChatClick = true;

        }//TextBlock_MouseDown

        private void Grid_MouseEnter(object sender, MouseEventArgs e) {

            if (this.SideBarColumn.Width.Value > 0)
                return;

            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

            thicknessAnimation.From = new Thickness(-33, 0, 0, 0);

            thicknessAnimation.To = new Thickness(0);

            thicknessAnimation.Duration = TimeSpan.FromSeconds(0.3);

            this.ChatButton_TextBlock.BeginAnimation(TextBlock.MarginProperty, thicknessAnimation);

        }//Grid_MouseEnter

        private void Grid_MouseLeave(object sender, MouseEventArgs e) {

            if (this.SideBarColumn.Width.Value > 0)
                return;

            ThicknessAnimation thicknessAnimation = new ThicknessAnimation();

            thicknessAnimation.From = new Thickness(0);

            thicknessAnimation.To = new Thickness(-33, 0, 0, 0);

            thicknessAnimation.Duration = TimeSpan.FromSeconds(0.3);

            this.ChatButton_TextBlock.BeginAnimation(TextBlock.MarginProperty, thicknessAnimation);

        }//Grid_MouseLeave

    }//MainWindow

}//ClientWPF
