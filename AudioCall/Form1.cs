using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Gui;
using NAudio.Wave;

namespace AudioCallApp
{
    public partial class Form1 : Form
    {
        private WaveIn waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider waveProvider;
        private TcpClient client;
        private TcpListener server;
        private NetworkStream stream;
        private bool isServer;
        private bool isConnected = false;
        private bool isMuted = true;

        public Form1()
        {
            InitializeComponent();
            InitializeAudioControls();
        }

        //Hoa
        private void InitializeAudioControls()
        {
            btnTalk.Enabled = false;
            btnEndCall.Enabled = false;
            btnConnect.BackColor = Color.LightGreen;
            btnEndCall.BackColor = Color.LightPink;
            btnTalk.BackColor = Color.LightGray;

            // Set initial volume
            volumeSlider.Minimum = 0;
            volumeSlider.Maximum = 100;
            volumeSlider.Value = 50;
            volumeSlider.Enabled = false;
        }

        //HOA
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                btnConnect.Enabled = false;
                statusLabel.Text = "Connecting...";

                try
                {
                    if (isServer)
                        await StartServerAsync();
                    else
                        await StartClientAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Connection failed";
                    btnConnect.Enabled = true;
                    return;
                }
            }
        }


        //Hoa
        private void btnTalk_MouseUp(object sender, MouseEventArgs e)
        {
            if (isConnected)
            {
                isMuted = true;
                waveIn.StopRecording();
                btnTalk.BackColor = Color.LightGray;
                statusLabel.Text = "Connected";
            }
        }

    }
}