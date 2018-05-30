using ScreenViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ClientWPF {
    /// <summary>
    /// Логика взаимодействия для ChatFileMessage.xaml
    /// </summary>
    public partial class ChatFileMessage : UserControl {

        public enum FileButtons {

            Delete = 0,
            Download = 1,

        }//FileButtons

        private FileButtons fileButton;

        MainWindow MainWindow;

        public void SendOK() {

            this.Dispatcher.Invoke(() => {

                this.FileButton.IsEnabled = true;

            });

        }//Downloaded

        public ChatFileMessage(bool IsSend, string FName, MainWindow mainWindow) {

            InitializeComponent();

            this.MainWindow = mainWindow;

            if (IsSend) {

                this.MainBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFAFFF9E"));

                this.MainBorder.HorizontalAlignment = HorizontalAlignment.Left;

                this.fileButton = FileButtons.Delete;

                this.FileButton.Content = "Delete";

                this.FileButton.IsEnabled = false;

            }//if
            else {

                this.MainBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF9E"));

                this.MainBorder.HorizontalAlignment = HorizontalAlignment.Right;

                this.fileButton = FileButtons.Download;

                this.FileButton.Content = "Download";

            }//else

            this.FileName.Text = FName;

        }//ChatFileMessage

        private void FileButton_Click(object sender, RoutedEventArgs e) {

            if(this.fileButton == FileButtons.Delete) {

                SCMessage message = new SCMessage { MessageType = SCMessageType.DeleteFileServer, Message = this.FileName.Text, From = this.MainWindow.connectionSettings.NickName };

                var bytes = SCMessage.SerializeJSON_ToBytes(message);

                this.MainWindow.TCP.GetStream().Write(bytes, 0, bytes.Length);

                this.MainWindow._ChatStackPanel.Children.Remove(this);

                return;

            }//if
            else if (this.fileButton == FileButtons.Download) {

                if(this.MainWindow.DownloadFile == true) {

                    MessageBox.Show("Wait Last Download File!");

                }//if
                else {

                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    var extension = System.IO.Path.GetExtension(this.FileName.Text);

                    saveFileDialog.FileName = this.FileName.Text;

                    saveFileDialog.Filter = $"({extension})|*{extension}";

                    var result = saveFileDialog.ShowDialog();

                    if (result != null) {

                        if (result.Value) {

                            this.MainWindow.DownloadFilePath = saveFileDialog.FileName;

                            SCMessage message = new SCMessage { MessageType = SCMessageType.SendFileName, Message = this.FileName.Text };

                            var bytes = SCMessage.SerializeJSON_ToBytes(message);

                            this.MainWindow.TCP.GetStream().Write(bytes, 0, bytes.Length);

                        }//if

                    }//if

                }//else

            }//else if

        }//FileButton_Click

        private void FileWrapPanel_Loaded(object sender, RoutedEventArgs e) {

            this.FileGrid.Width = this.FileButton.ActualWidth + this.FileName.ActualWidth + 20;

            this.FileGrid.ColumnDefinitions[0].Width = new GridLength(this.FileButton.ActualWidth + 10);

            this.FileGrid.ColumnDefinitions[1].Width = new GridLength(this.FileName.ActualWidth + 10);

        }//FileWrapPanel_Loaded

    }//ChatFileMessage

}//ClientWPF
