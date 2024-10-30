using System.Drawing;
using System.Windows.Forms;
using System;

namespace AudioCallApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnConnect;
        private Button btnEndCall;
        private Button btnTalk;
        private TextBox txtServerIP;
        private Label lblServerIP;
        private Label statusLabel;
        private TrackBar volumeSlider;
        private Label lblVolume;
        private RadioButton rbServer;
        private RadioButton rbClient;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Khởi tạo các control và đặt các thuộc tính của chúng

            this.components = new System.ComponentModel.Container();
            this.btnConnect = new Button();
            this.btnEndCall = new Button();
            this.btnTalk = new Button();
            this.txtServerIP = new TextBox();
            this.lblServerIP = new Label();
            this.statusLabel = new Label();
            this.volumeSlider = new TrackBar();
            this.lblVolume = new Label();
            this.rbServer = new RadioButton();
            this.rbClient = new RadioButton();

            // Form1
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnEndCall);
            this.Controls.Add(this.btnTalk);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.volumeSlider);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.rbServer);
            this.Controls.Add(this.rbClient);
            this.Text = "Audio Call App";

            // btnConnect
            this.btnConnect.Location = new Point(20, 20);
            this.btnConnect.Size = new Size(100, 30);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new EventHandler(this.btnConnect_Click);

            // btnEndCall
            this.btnEndCall.Location = new Point(20, 60);
            this.btnEndCall.Size = new Size(100, 30);
            this.btnEndCall.Text = "End Call";
            this.btnEndCall.Click += new EventHandler(this.btnEndCall_Click);

            // btnTalk
            this.btnTalk.Location = new Point(20, 100);
            this.btnTalk.Size = new Size(100, 30);
            this.btnTalk.Text = "Talk";
            this.btnTalk.Click += new EventHandler(this.btnTalk_Click);

            // txtServerIP
            this.txtServerIP.Location = new Point(150, 20);
            this.txtServerIP.Size = new Size(200, 30);

            // lblServerIP
            this.lblServerIP.Location = new Point(150, 60);
            this.lblServerIP.Size = new Size(100, 30);
            this.lblServerIP.Text = "Server IP:";

            // statusLabel
            this.statusLabel.Location = new Point(150, 100);
            this.statusLabel.Size = new Size(200, 30);
            this.statusLabel.Text = "Status: Disconnected";

            // volumeSlider
            this.volumeSlider.Location = new Point(150, 140);
            this.volumeSlider.Size = new Size(200, 45);
            this.volumeSlider.Minimum = 0;
            this.volumeSlider.Maximum = 100;
            this.volumeSlider.Value = 50;

            // lblVolume
            this.lblVolume.Location = new Point(150, 190);
            this.lblVolume.Size = new Size(100, 30);
            this.lblVolume.Text = "Volume";

            // RadioButton Server
            this.rbServer.Location = new Point(20, 150);
            this.rbServer.Size = new Size(100, 30);
            this.rbServer.Text = "Server";
            this.rbServer.Checked = true; // Mặc định là server
            this.rbServer.CheckedChanged += new EventHandler(this.rbServer_CheckedChanged); // Gán sự kiện đã định nghĩa trong Form1.cs

            // RadioButton Client
            this.rbClient.Location = new Point(130, 150);
            this.rbClient.Size = new Size(100, 30);
            this.rbClient.Text = "Client";
            this.rbClient.CheckedChanged += new EventHandler(this.rbClient_CheckedChanged); // Gán sự kiện đã định nghĩa trong Form1.cs

            ((System.ComponentModel.ISupportInitialize)(this.volumeSlider)).EndInit();
        }
    }
}
