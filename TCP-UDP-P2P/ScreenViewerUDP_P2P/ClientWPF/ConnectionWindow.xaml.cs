using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using ScreenViewer;

namespace ClientWPF {
    /// <summary>
    /// Логика взаимодействия для ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window {

        public ConnectionSettings connectionSettings { get; set; }

        const string FName = "ScreenViewerClientSettings.json";

        public ConnectionWindow() {

            InitializeComponent();

            this.connectionSettings = new ConnectionSettings();

            if (File.Exists(FName)) {

                try {

                    var settings = ConnectionSettings.DeserializeJSON(FName);

                    this.IP_TextBox.Text = settings.IP;

                    this.Port_TextBox.Text = settings.Port.ToString();

                    this.Video_Send_Port_TextBox.Text = settings.VideoSendPort.ToString();

                    this.Audio_Send_Port_TextBox.Text = settings.AudioSendPort.ToString();

                    this.RandomSend_CheckBox.IsChecked = settings.RandomSend;

                    this.ShowBrokenImage_CheckBox.IsChecked = settings.ShowBrokenImage;

                    this.Password_PasswordBox.Password = EncryptionString.Decrypt(settings.ServerPassword);

                    this.NickName_TextBox.Text = settings.NickName;

                }//try
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);

                }//catch

            }//if
            else {

                this.RandomSend_CheckBox.IsChecked = true;

            }//else

        }//ConnectionWindow

        private void Button_Click(object sender, RoutedEventArgs e) {

            try {

                IPAddress.Parse(this.IP_TextBox.Text);

                this.connectionSettings.IP = this.IP_TextBox.Text;

            }//try
            catch {

                MessageBox.Show("Error IP Parse", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }//catch

            if(this.Port_TextBox.Text.Length == 0) {

                MessageBox.Show("Port is Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }//if

            int port = 0;

            try {

                port = int.Parse(this.Port_TextBox.Text);

            }//try
            catch {

                MessageBox.Show("Error Port Parse", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }//catch


            if(port > 65535) {

                MessageBox.Show("Port Max Value = 65535", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }//if
            else if(port == 0) {

                MessageBox.Show("Port Min Value = 1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }//else if

            this.connectionSettings.Port = port;

            string NickName = this.NickName_TextBox.Text;

            if(NickName.Trim().Length == 0) {

                MessageBox.Show("NickName should not be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;

            }//if

            NickName = NickName.Replace(" ", "");

            if (this.connectionSettings.RandomSend == false) {

                if (this.Video_Send_Port_TextBox.Text.Length == 0) {

                    MessageBox.Show("Video Send Port is Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//if

                try {

                    port = int.Parse(this.Video_Send_Port_TextBox.Text);

                }//try
                catch {

                    MessageBox.Show("Error Video Send Port Parse", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//catch

                if (port > 65535) {

                    MessageBox.Show("Video Send Port Max Value = 65535", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//if
                else if (port == 0) {

                    MessageBox.Show("Video Send Port Min Value = 1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//else if

                this.connectionSettings.VideoSendPort = port;

                if (this.Audio_Send_Port_TextBox.Text.Length == 0) {

                    MessageBox.Show("Audio Send Port is Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//if

                try {


                    port = int.Parse(this.Audio_Send_Port_TextBox.Text);

                }//try
                catch {

                    MessageBox.Show("Error Audio Send Port Parse", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//catch

                if (port > 65535) {

                    MessageBox.Show("Audio Send Port Max Value = 65535", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//if
                else if (port == 0) {

                    MessageBox.Show("Audio Send Port Min Value = 1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;

                }//else if

                this.connectionSettings.AudioSendPort = port;

            }//if

            this.connectionSettings.ServerPassword = EncryptionString.Encrypt(this.Password_PasswordBox.Password);

            this.connectionSettings.NickName = NickName;

            try {

                ConnectionSettings.SerializeJSON(this.connectionSettings, FName);

            }//try
            catch (Exception ex) {

                MessageBox.Show(ex.Message);

            }//catch

            this.DialogResult = true;

        }//Button_Click

        private void Port_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {

            if (e.Text.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }) == -1)
                e.Handled = true;
            else
                e.Handled = false;

        }//Port_TextBox_PreviewTextInput

        private void IP_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {

            if (e.Text.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' }) == -1)
                e.Handled = true;
            else
                e.Handled = false;

        }//IP_TextBox_PreviewTextInput

        private void RandomSend_CheckBox_Checked(object sender, RoutedEventArgs e) {

            this.connectionSettings.RandomSend = true;

            this.Video_Send_Port_TextBox.IsEnabled = false;

            this.Audio_Send_Port_TextBox.IsEnabled = false;

            this.Video_Send_Port_TextBox.Text = string.Empty;

            this.Audio_Send_Port_TextBox.Text = string.Empty;

        }//CheckBox_Checked

        private void RandomSend_CheckBox_Unchecked(object sender, RoutedEventArgs e) {

            this.connectionSettings.RandomSend = false;

            this.Video_Send_Port_TextBox.IsEnabled = true;

            this.Audio_Send_Port_TextBox.IsEnabled = true;

        }//RandomSend_CheckBox_Unchecked

        private void ShowBrokenImageCheckBox_Checked(object sender, RoutedEventArgs e) {

            this.connectionSettings.ShowBrokenImage = true;

        }//ShowBrokenImageCheckBox_Checked

        private void ShowBrokenImageCheckBox_Unchecked(object sender, RoutedEventArgs e) {

            this.connectionSettings.ShowBrokenImage = false;

        }//ShowBrokenImageCheckBox_Unchecked

        private void Window_PreviewMouseRightButton(object sender, MouseButtonEventArgs e) {

            e.Handled = true;

        }//Window_PreviewMouseRightButtonDown

    }//ConnectionWindow

}//ClientWPF
