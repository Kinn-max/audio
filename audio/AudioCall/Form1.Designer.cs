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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // Giải phóng tài nguyên âm thanh và các đối tượng liên quan
                if (waveFileWriter != null)
                {
                    waveFileWriter.Dispose();
                    waveFileWriter = null;
                }

                if (waveIn != null)
                {
                    waveIn.Dispose();
                    waveIn = null;
                }

                if (waveOut != null)
                {
                    waveOut.Dispose();
                    waveOut = null;
                }

                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }

                if (client != null)
                {
                    client.Close();
                    client = null;
                }

                if (server != null)
                {
                    server.Stop();
                    server = null;
                }
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnEndCall = new System.Windows.Forms.Button();
            this.btnTalk = new System.Windows.Forms.Button();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.volumeSlider = new System.Windows.Forms.TrackBar();
            this.lblVolume = new System.Windows.Forms.Label();
            this.rbServer = new System.Windows.Forms.RadioButton();
            this.rbClient = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.volumeSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(20, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 30);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnEndCall
            // 
            this.btnEndCall.Location = new System.Drawing.Point(20, 60);
            this.btnEndCall.Name = "btnEndCall";
            this.btnEndCall.Size = new System.Drawing.Size(100, 30);
            this.btnEndCall.TabIndex = 1;
            this.btnEndCall.Text = "End Call";
            this.btnEndCall.Click += new System.EventHandler(this.btnEndCall_Click);
            // 
            // btnTalk
            // 
            this.btnTalk.Location = new System.Drawing.Point(20, 100);
            this.btnTalk.Name = "btnTalk";
            this.btnTalk.Size = new System.Drawing.Size(100, 30);
            this.btnTalk.TabIndex = 2;
            this.btnTalk.Text = "Talk";
            this.btnTalk.Click += new System.EventHandler(this.btnTalk_Click);
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(150, 20);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(200, 29);
            this.txtServerIP.TabIndex = 3;
            // 
            // lblServerIP
            // 
            this.lblServerIP.Location = new System.Drawing.Point(150, 60);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(100, 30);
            this.lblServerIP.TabIndex = 4;
            this.lblServerIP.Text = "Server IP:";
            // 
            // statusLabel
            // 
            this.statusLabel.Location = new System.Drawing.Point(150, 100);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(200, 30);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "Status: Disconnected";
            // 
            // volumeSlider
            // 
            this.volumeSlider.Location = new System.Drawing.Point(150, 140);
            this.volumeSlider.Maximum = 100;
            this.volumeSlider.Name = "volumeSlider";
            this.volumeSlider.Size = new System.Drawing.Size(200, 80);
            this.volumeSlider.TabIndex = 6;
            this.volumeSlider.Value = 50;
            // 
            // lblVolume
            // 
            this.lblVolume.Location = new System.Drawing.Point(150, 190);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(100, 30);
            this.lblVolume.TabIndex = 7;
            this.lblVolume.Text = "Volume";
            // 
            // rbServer
            // 
            this.rbServer.Checked = true;
            this.rbServer.Location = new System.Drawing.Point(20, 150);
            this.rbServer.Name = "rbServer";
            this.rbServer.Size = new System.Drawing.Size(100, 30);
            this.rbServer.TabIndex = 8;
            this.rbServer.TabStop = true;
            this.rbServer.Text = "Server";
            this.rbServer.CheckedChanged += new System.EventHandler(this.rbServer_CheckedChanged);
            // 
            // rbClient
            // 
            this.rbClient.Location = new System.Drawing.Point(20, 201);
            this.rbClient.Name = "rbClient";
            this.rbClient.Size = new System.Drawing.Size(100, 30);
            this.rbClient.TabIndex = 9;
            this.rbClient.Text = "Client";
            this.rbClient.CheckedChanged += new System.EventHandler(this.rbClient_CheckedChanged);
            // 
            // Form1
            // 
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
            this.Name = "Form1";
            this.Text = "Audio Call App";
            ((System.ComponentModel.ISupportInitialize)(this.volumeSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
