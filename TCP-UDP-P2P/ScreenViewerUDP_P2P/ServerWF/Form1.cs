using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using NAudio.Wave;
using System.IO;
using System.Threading;
using ScreenViewer;

namespace ServerWF {
    public partial class Form1 : Form {

        public ConsoleControl.ConsoleControl Console { get => this.СonsoleControl; }

        public string Title { get => this.Text; set => this.Text = value; }

        ScreenViewerServer server;

        const string FName = "ScreenViewerServerSettings.json";

        public string oldTitle;

        string InputText = string.Empty;

        bool copyFile = false;

        public Form1() {

            InitializeComponent();

            this.oldTitle = this.Text;

            this.Password_textBox.PasswordChar = '*';

            var hosts = Dns.GetHostEntry(Dns.GetHostName());

            var ips = hosts.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToList();

            foreach(var ip in ips) {

                this.IP_comboBox.Items.Add(ip.ToString());

            }//foreach

            this.IP_comboBox.SelectedIndex = 0;

            this.Video_comboBox.Items.Add("GDI");
            this.Video_comboBox.Items.Add("DirectX11");

            this.Video_comboBox.SelectedIndex = 0;

            int waveInDevices = WaveIn.DeviceCount;

            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++) {

                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);

                this.Audio_comboBox.Items.Add(deviceInfo.ProductName);

            }//for

            if (waveInDevices != 0)
                this.Audio_comboBox.SelectedIndex = 0;

            try {

                if (File.Exists(FName)){

                    var settings = SVServerSettings.DeserializeJSON(FName);

                    this.TCP_Port_numericUpDown.Value = settings.TCP_Port;

                    this.Video_comboBox.SelectedIndex = settings.VideoIndex;

                    this.Delay_numericUpDown.Value = settings.Delay;

                    this.AutoDelay_checkBox.Checked = settings.AutoDelay;

                    this.Audio_checkBox.Checked = settings.AudioEnable;

                    this.Video_UDP_numericUpDown.Value = settings.Video_UPD_Port;

                    this.Audio_UDP_numericUpDown.Value = settings.Audio_UPD_Port;

                    this.OnlyWindow_checkBox.Checked = settings.OnlyWindowScreen;

                    this.FileSend_TCP_numericUpDown.Value = settings.FileSend_TCP_Port;

                    this.Password_textBox.Text = EncryptionString.Decrypt(settings.ServerPassword);

                    for(int i = 0; i < this.IP_comboBox.Items.Count; i++) {

                        if(this.IP_comboBox.Items[i].ToString() == settings.IP) {

                            this.IP_comboBox.SelectedIndex = i;

                            break;

                        }//if

                    }//for

                    if(settings.Audio != null) {

                        for (int i = 0; i < this.IP_comboBox.Items.Count; i++) {

                            if (this.Audio_comboBox.Items[i].ToString() == settings.Audio) {

                                this.Audio_comboBox.SelectedIndex = i;

                                break;

                            }//if

                        }//for

                    }//if

                }//if

            }//try
            catch(Exception ex) {

                MessageBox.Show(ex.Message);

            }//catch

        }//Form1

        private void Start_button_Click(object sender, EventArgs e) {

            this.Start_button.Enabled = false;

            if(this.server == null) {

                try {

                    ScreenShotMethod method = this.Video_comboBox.SelectedIndex == 0 ? ScreenShotMethod.GDI : ScreenShotMethod.DirectX11;

                    var ip = IPAddress.Parse(this.IP_comboBox.SelectedItem.ToString());

                    var TCP_Port = (int)this.TCP_Port_numericUpDown.Value;

                    var UDP_Video_Port = (int)this.Video_UDP_numericUpDown.Value;

                    int Sleep;

                    if (this.AutoDelay_checkBox.Checked)
                        Sleep = -1;
                    else
                        Sleep = (int)this.Delay_numericUpDown.Value;

                    string Password = this.Password_textBox.Text;

                    int FSendTCP = (int)this.FileSend_TCP_numericUpDown.Value;

                    if (this.Audio_checkBox.Checked) {

                        var UDP_Audio_Port = (int)this.Audio_UDP_numericUpDown.Value;

                        int DeviceNumber = this.Audio_comboBox.SelectedIndex;

                        this.server = new ScreenViewerServer(FSendTCP, this.OnlyWindow_checkBox.Checked, Password, Sleep, method, ip, TCP_Port, UDP_Video_Port, UDP_Audio_Port, DeviceNumber);

                    }//if
                    else {

                        this.server = new ScreenViewerServer(FSendTCP, this.OnlyWindow_checkBox.Checked, Password, Sleep, method, ip, TCP_Port, UDP_Video_Port);

                    }//else

                    this.server.Start();

                    SVServerSettings settings = new SVServerSettings();

                    settings.TCP_Port = (int)this.TCP_Port_numericUpDown.Value;

                    settings.VideoIndex = this.Video_comboBox.SelectedIndex;

                    settings.Delay = (int)this.Delay_numericUpDown.Value ;

                    settings.AutoDelay = this.AutoDelay_checkBox.Checked;

                    settings.AudioEnable = this.Audio_checkBox.Checked;

                    settings.Audio = this.Audio_comboBox.SelectedItem != null ? this.Audio_comboBox.SelectedItem.ToString() : null;

                    settings.Video_UPD_Port = (int)this.Video_UDP_numericUpDown.Value;

                    settings.Audio_UPD_Port = (int)this.Audio_UDP_numericUpDown.Value;

                    settings.IP = this.IP_comboBox.SelectedItem.ToString();

                    settings.FileSend_TCP_Port = (int)this.FileSend_TCP_numericUpDown.Value;

                    settings.OnlyWindowScreen = this.OnlyWindow_checkBox.Checked;

                    settings.ServerPassword = EncryptionString.Encrypt(this.Password_textBox.Text);

                    SVServerSettings.SerializeJSON(settings, FName);

                }//try
                catch(Exception ex) {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }//catch

            }//if

            Task.Run(() => {

                Thread.Sleep(2000);

                this.Invoke((Action)(() => { this.Stop_button.Enabled = true; }));

            });

        }//Start_button_Click

        private void Stop_button_Click(object sender, EventArgs e) {

            this.Stop_button.Enabled = false;

            this.Text = this.oldTitle;

            if (this.server != null) {

                this.server.Dispose();

                this.server = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }//if

            Task.Run(() => {

                Thread.Sleep(2000);

                this.Invoke((Action)(() => { this.Start_button.Enabled = true; }));

            });            

        }//Stop_button_Click

        private void Audio_checkBox_CheckedChanged(object sender, EventArgs e) {

            if (this.Audio_checkBox.Checked) {

                if(this.Audio_comboBox.Items.Count == 0) {

                    MessageBox.Show("Recording devices not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    this.Audio_checkBox.Checked = false;

                    return;

                }//if

                this.Audio_comboBox.Enabled = true;

                this.Audio_UDP_numericUpDown.Enabled = true;

            }//if
            else {

                this.Audio_comboBox.Enabled = false;

                this.Audio_UDP_numericUpDown.Enabled = false;

            }//else

        }//Audio_checkBox_CheckedChanged

        private void AutoDelay_checkBox_CheckedChanged(object sender, EventArgs e) {

            if (this.AutoDelay_checkBox.Checked) {

                this.Delay_numericUpDown.Enabled = false;

                this.Delay_numericUpDown.Value = 0;

            }//if
            else {

                this.Delay_numericUpDown.Enabled = true;

            }//else

        }//AutoDelay_checkBox_CheckedChanged

        private void ClearConsole_button_Click(object sender, EventArgs e) {

            this.Console.Clear();

        }//ClearConsole_button_Click

        private void Logger_checkBox_CheckedChanged(object sender, EventArgs e) {

        }//Logger_checkBox_CheckedChanged

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

            if (this.server != null)
                this.server.Dispose();

            if (!Directory.Exists(Program.SV_Directory)) {

                Directory.CreateDirectory(Program.SV_Directory);

                return;

            }//if

            var files = Directory.GetFiles(Program.SV_Directory);

            foreach(var file in files) {

                try {

                    File.Delete(file);

                }//try
                catch {

                }//catch

            }//foreache            

        }//Form1_FormClosing

        private void Video_comboBox_SelectedIndexChanged(object sender, EventArgs e) {

            if (this.Video_comboBox.SelectedIndex == 1) {

                this.OnlyWindow_checkBox.Checked = false;

                this.OnlyWindow_checkBox.Enabled = false;

            }//if
            else
                this.OnlyWindow_checkBox.Enabled = true;

        }//Video_comboBox_SelectedIndexChanged

        private void СonsoleControl_KeyDown(object sender, KeyEventArgs e) {

            if(this.server != null) {

                if (e.KeyCode == Keys.ShiftKey)
                    return;

                if (e.KeyCode == Keys.Back) {

                    if(this.InputText.Length >= 1) {

                        this.InputText = this.InputText.Substring(0, this.InputText.Length - 1);

                    }//if

                }//if
                else if (e.KeyCode == Keys.Enter) {

                    if(this.InputText.Trim().Length > 0) {

                        this.server.SendServerMessage(this.InputText.Trim());

                    }//if

                    this.InputText = string.Empty;

                }//else if
                else
                    this.InputText += WIN32.GetCharFromKey(e.KeyValue);

            }//if

        }//СonsoleControl_KeyDown

        private void СonsoleControl_DragEnter(object sender, DragEventArgs e) {

            if (this.server == null || this.copyFile)
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;

        }//СonsoleControl_DragEnter

        private void СonsoleControl_DragDrop(object sender, DragEventArgs e) {

            if(this.server != null && this.copyFile == false) {

                this.copyFile = true;

                if (e.Data.GetDataPresent(DataFormats.FileDrop)) {

                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (!File.Exists(files[0]))
                        return;

                    var fName = files[0];

                    Task.Run(() => {

                        if (!Directory.Exists("ScreenServerFiles")) {

                            Directory.CreateDirectory("ScreenServerFiles");

                        }//i

                        var newFname = $"{DateTime.Now.Ticks}_{Path.GetFileName(fName)}";

                        File.Copy(fName, $"ScreenServerFiles/{newFname}");

                        this.server.SendFileServer(newFname);

                        this.Invoke((Action)(() => {

                            Program.ConsoleWriteLine($"{newFname}", Color.LimeGreen, Color.Black);

                        }));                      

                        this.copyFile = false;

                    });

                }//if

            }//if

        }//СonsoleControl_DragDrop

    }//Form1

}//ServerWF
