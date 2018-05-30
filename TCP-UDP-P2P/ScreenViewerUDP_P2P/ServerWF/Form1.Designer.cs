namespace ServerWF {
    partial class Form1 {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.FileSend_TCP_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.OnlyWindow_checkBox = new System.Windows.Forms.CheckBox();
            this.Password_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ClearConsole_button = new System.Windows.Forms.Button();
            this.Stop_button = new System.Windows.Forms.Button();
            this.AutoDelay_checkBox = new System.Windows.Forms.CheckBox();
            this.Start_button = new System.Windows.Forms.Button();
            this.Delay_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.Audio_UDP_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Video_UDP_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Audio_comboBox = new System.Windows.Forms.ComboBox();
            this.Audio_checkBox = new System.Windows.Forms.CheckBox();
            this.Video_comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TCP_Port_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.IP_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.СonsoleControl = new ConsoleControl.ConsoleControl();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileSend_TCP_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Delay_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Audio_UDP_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Video_UDP_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TCP_Port_numericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.FileSend_TCP_numericUpDown);
            this.groupBox.Controls.Add(this.label8);
            this.groupBox.Controls.Add(this.OnlyWindow_checkBox);
            this.groupBox.Controls.Add(this.Password_textBox);
            this.groupBox.Controls.Add(this.label7);
            this.groupBox.Controls.Add(this.ClearConsole_button);
            this.groupBox.Controls.Add(this.Stop_button);
            this.groupBox.Controls.Add(this.AutoDelay_checkBox);
            this.groupBox.Controls.Add(this.Start_button);
            this.groupBox.Controls.Add(this.Delay_numericUpDown);
            this.groupBox.Controls.Add(this.label6);
            this.groupBox.Controls.Add(this.Audio_UDP_numericUpDown);
            this.groupBox.Controls.Add(this.label5);
            this.groupBox.Controls.Add(this.Video_UDP_numericUpDown);
            this.groupBox.Controls.Add(this.label4);
            this.groupBox.Controls.Add(this.Audio_comboBox);
            this.groupBox.Controls.Add(this.Audio_checkBox);
            this.groupBox.Controls.Add(this.Video_comboBox);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Controls.Add(this.TCP_Port_numericUpDown);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.IP_comboBox);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Location = new System.Drawing.Point(12, 13);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(216, 404);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Settings";
            // 
            // FileSend_TCP_numericUpDown
            // 
            this.FileSend_TCP_numericUpDown.Location = new System.Drawing.Point(100, 315);
            this.FileSend_TCP_numericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.FileSend_TCP_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FileSend_TCP_numericUpDown.Name = "FileSend_TCP_numericUpDown";
            this.FileSend_TCP_numericUpDown.Size = new System.Drawing.Size(92, 20);
            this.FileSend_TCP_numericUpDown.TabIndex = 22;
            this.FileSend_TCP_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileSend_TCP_numericUpDown.Value = new decimal(new int[] {
            7780,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 317);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "File TCP Port :";
            // 
            // OnlyWindow_checkBox
            // 
            this.OnlyWindow_checkBox.AutoSize = true;
            this.OnlyWindow_checkBox.Location = new System.Drawing.Point(10, 156);
            this.OnlyWindow_checkBox.Name = "OnlyWindow_checkBox";
            this.OnlyWindow_checkBox.Size = new System.Drawing.Size(153, 17);
            this.OnlyWindow_checkBox.TabIndex = 20;
            this.OnlyWindow_checkBox.Text = "Screen Only Activ Window";
            this.OnlyWindow_checkBox.UseVisualStyleBackColor = true;
            // 
            // Password_textBox
            // 
            this.Password_textBox.Location = new System.Drawing.Point(73, 21);
            this.Password_textBox.MaxLength = 20;
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.Size = new System.Drawing.Size(119, 20);
            this.Password_textBox.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Password :";
            // 
            // ClearConsole_button
            // 
            this.ClearConsole_button.Location = new System.Drawing.Point(9, 372);
            this.ClearConsole_button.Name = "ClearConsole_button";
            this.ClearConsole_button.Size = new System.Drawing.Size(191, 25);
            this.ClearConsole_button.TabIndex = 17;
            this.ClearConsole_button.Text = "ClearConsole";
            this.ClearConsole_button.UseVisualStyleBackColor = true;
            this.ClearConsole_button.Click += new System.EventHandler(this.ClearConsole_button_Click);
            // 
            // Stop_button
            // 
            this.Stop_button.Enabled = false;
            this.Stop_button.Location = new System.Drawing.Point(108, 341);
            this.Stop_button.Name = "Stop_button";
            this.Stop_button.Size = new System.Drawing.Size(92, 25);
            this.Stop_button.TabIndex = 4;
            this.Stop_button.Text = "Stop";
            this.Stop_button.UseVisualStyleBackColor = true;
            this.Stop_button.Click += new System.EventHandler(this.Stop_button_Click);
            // 
            // AutoDelay_checkBox
            // 
            this.AutoDelay_checkBox.AutoSize = true;
            this.AutoDelay_checkBox.Location = new System.Drawing.Point(8, 220);
            this.AutoDelay_checkBox.Name = "AutoDelay_checkBox";
            this.AutoDelay_checkBox.Size = new System.Drawing.Size(75, 17);
            this.AutoDelay_checkBox.TabIndex = 14;
            this.AutoDelay_checkBox.Text = "AutoDelay";
            this.AutoDelay_checkBox.UseVisualStyleBackColor = true;
            this.AutoDelay_checkBox.CheckedChanged += new System.EventHandler(this.AutoDelay_checkBox_CheckedChanged);
            // 
            // Start_button
            // 
            this.Start_button.Location = new System.Drawing.Point(8, 341);
            this.Start_button.Name = "Start_button";
            this.Start_button.Size = new System.Drawing.Size(94, 25);
            this.Start_button.TabIndex = 2;
            this.Start_button.Text = "Start";
            this.Start_button.UseVisualStyleBackColor = true;
            this.Start_button.Click += new System.EventHandler(this.Start_button_Click);
            // 
            // Delay_numericUpDown
            // 
            this.Delay_numericUpDown.Location = new System.Drawing.Point(136, 218);
            this.Delay_numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Delay_numericUpDown.Name = "Delay_numericUpDown";
            this.Delay_numericUpDown.Size = new System.Drawing.Size(56, 20);
            this.Delay_numericUpDown.TabIndex = 13;
            this.Delay_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(89, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Delay :";
            // 
            // Audio_UDP_numericUpDown
            // 
            this.Audio_UDP_numericUpDown.Enabled = false;
            this.Audio_UDP_numericUpDown.Location = new System.Drawing.Point(100, 283);
            this.Audio_UDP_numericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.Audio_UDP_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Audio_UDP_numericUpDown.Name = "Audio_UDP_numericUpDown";
            this.Audio_UDP_numericUpDown.Size = new System.Drawing.Size(92, 20);
            this.Audio_UDP_numericUpDown.TabIndex = 11;
            this.Audio_UDP_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Audio_UDP_numericUpDown.Value = new decimal(new int[] {
            7778,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 285);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Audio UDP Port :";
            // 
            // Video_UDP_numericUpDown
            // 
            this.Video_UDP_numericUpDown.Location = new System.Drawing.Point(93, 185);
            this.Video_UDP_numericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.Video_UDP_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Video_UDP_numericUpDown.Name = "Video_UDP_numericUpDown";
            this.Video_UDP_numericUpDown.Size = new System.Drawing.Size(95, 20);
            this.Video_UDP_numericUpDown.TabIndex = 9;
            this.Video_UDP_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Video_UDP_numericUpDown.Value = new decimal(new int[] {
            7777,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Video UDP Port :";
            // 
            // Audio_comboBox
            // 
            this.Audio_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Audio_comboBox.Enabled = false;
            this.Audio_comboBox.FormattingEnabled = true;
            this.Audio_comboBox.Location = new System.Drawing.Point(74, 249);
            this.Audio_comboBox.Name = "Audio_comboBox";
            this.Audio_comboBox.Size = new System.Drawing.Size(118, 21);
            this.Audio_comboBox.TabIndex = 7;
            // 
            // Audio_checkBox
            // 
            this.Audio_checkBox.AutoSize = true;
            this.Audio_checkBox.Location = new System.Drawing.Point(9, 252);
            this.Audio_checkBox.Name = "Audio_checkBox";
            this.Audio_checkBox.Size = new System.Drawing.Size(59, 17);
            this.Audio_checkBox.TabIndex = 6;
            this.Audio_checkBox.Text = "Audio :";
            this.Audio_checkBox.UseVisualStyleBackColor = true;
            this.Audio_checkBox.CheckedChanged += new System.EventHandler(this.Audio_checkBox_CheckedChanged);
            // 
            // Video_comboBox
            // 
            this.Video_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Video_comboBox.FormattingEnabled = true;
            this.Video_comboBox.Location = new System.Drawing.Point(48, 122);
            this.Video_comboBox.Name = "Video_comboBox";
            this.Video_comboBox.Size = new System.Drawing.Size(140, 21);
            this.Video_comboBox.TabIndex = 5;
            this.Video_comboBox.SelectedIndexChanged += new System.EventHandler(this.Video_comboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Video :";
            // 
            // TCP_Port_numericUpDown
            // 
            this.TCP_Port_numericUpDown.Location = new System.Drawing.Point(125, 90);
            this.TCP_Port_numericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.TCP_Port_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TCP_Port_numericUpDown.Name = "TCP_Port_numericUpDown";
            this.TCP_Port_numericUpDown.Size = new System.Drawing.Size(67, 20);
            this.TCP_Port_numericUpDown.TabIndex = 3;
            this.TCP_Port_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TCP_Port_numericUpDown.Value = new decimal(new int[] {
            7777,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Connection TCP Port :";
            // 
            // IP_comboBox
            // 
            this.IP_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IP_comboBox.FormattingEnabled = true;
            this.IP_comboBox.Location = new System.Drawing.Point(32, 54);
            this.IP_comboBox.Name = "IP_comboBox";
            this.IP_comboBox.Size = new System.Drawing.Size(160, 21);
            this.IP_comboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.СonsoleControl);
            this.groupBox2.Location = new System.Drawing.Point(234, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(660, 404);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Console";
            // 
            // СonsoleControl
            // 
            this.СonsoleControl.AllowDrop = true;
            this.СonsoleControl.AllowInput = true;
            this.СonsoleControl.BackColor = System.Drawing.Color.Black;
            this.СonsoleControl.ConsoleBackgroundColor = System.Drawing.Color.Black;
            this.СonsoleControl.ConsoleForegroundColor = System.Drawing.Color.LightGray;
            this.СonsoleControl.CurrentBackgroundColor = System.Drawing.Color.Black;
            this.СonsoleControl.CurrentForegroundColor = System.Drawing.Color.LimeGreen;
            this.СonsoleControl.CursorType = ConsoleControl.CursorTypes.Underline;
            this.СonsoleControl.EchoInput = true;
            this.СonsoleControl.ForeColor = System.Drawing.Color.LightGray;
            this.СonsoleControl.Location = new System.Drawing.Point(6, 19);
            this.СonsoleControl.Name = "СonsoleControl";
            this.СonsoleControl.ShowCursor = true;
            this.СonsoleControl.Size = new System.Drawing.Size(646, 377);
            this.СonsoleControl.TabIndex = 0;
            this.СonsoleControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.СonsoleControl_DragDrop);
            this.СonsoleControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.СonsoleControl_DragEnter);
            this.СonsoleControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.СonsoleControl_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 428);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Screen Viewer Server by SG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileSend_TCP_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Delay_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Audio_UDP_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Video_UDP_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TCP_Port_numericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ComboBox IP_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Video_UDP_numericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Audio_comboBox;
        private System.Windows.Forms.CheckBox Audio_checkBox;
        private System.Windows.Forms.ComboBox Video_comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TCP_Port_numericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Audio_UDP_numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Start_button;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Stop_button;
        private System.Windows.Forms.CheckBox AutoDelay_checkBox;
        private System.Windows.Forms.NumericUpDown Delay_numericUpDown;
        private System.Windows.Forms.Label label6;
        private ConsoleControl.ConsoleControl СonsoleControl;
        private System.Windows.Forms.Button ClearConsole_button;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox OnlyWindow_checkBox;
        private System.Windows.Forms.NumericUpDown FileSend_TCP_numericUpDown;
        private System.Windows.Forms.Label label8;
    }
}

