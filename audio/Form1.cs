using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using NAudio.Wave;

namespace AudioCall2
{
    public partial class Form1 : Form
    {
        private WaveIn waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider waveProvider;
        private TcpClient client;
        private TcpListener server;
        private NetworkStream stream;
        private bool isServer = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStartCall_Click(object sender, EventArgs e)
        {
            if (isServer)
            {
                StartServer();
            }
            else
            {
                StartClient();
            }
        }

        private void btnStopCall_Click(object sender, EventArgs e)
        {
            StopCall();
        }

        private void StartServer()
        {
            server = new TcpListener(IPAddress.Any, 8000);
            server.Start();
            server.BeginAcceptTcpClient(AcceptClientCallback, server);
            MessageBox.Show("Server started, waiting for connection...");
        }

        private void AcceptClientCallback(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            client = listener.EndAcceptTcpClient(ar);
            stream = client.GetStream();
            MessageBox.Show("Client connected!");

            // Bắt đầu nhận âm thanh
            StartReceivingAudio();
        }

        private void StartClient()
        {
            try
            {
                // Sử dụng IP từ TextBox để kết nối đến server
                string serverIp = txtServerIP.Text;
                client = new TcpClient(serverIp, 8000);
                stream = client.GetStream();
                MessageBox.Show("Connected to server!");

                // Bắt đầu gửi âm thanh
                StartSendingAudio();
                StartReceivingAudio();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void StartSendingAudio()
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(44100, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (stream != null && stream.CanWrite)
            {
                stream.Write(e.Buffer, 0, e.BytesRecorded);
            }
        }

        private void StartReceivingAudio()
        {
            waveOut = new WaveOut();
            waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
            waveOut.Init(waveProvider);
            waveOut.Play();

            byte[] buffer = new byte[4096];
            stream.BeginRead(buffer, 0, buffer.Length, AudioReceivedCallback, buffer);
        }

        private void AudioReceivedCallback(IAsyncResult ar)
        {
            int bytesRead = stream.EndRead(ar);
            byte[] buffer = (byte[])ar.AsyncState;

            if (bytesRead > 0)
            {
                waveProvider.AddSamples(buffer, 0, bytesRead);
            }

            stream.BeginRead(buffer, 0, buffer.Length, AudioReceivedCallback, buffer);
        }

        private void StopCall()
        {
            waveIn?.StopRecording();
            waveOut?.Stop();
            stream?.Close();
            client?.Close();
            server?.Stop();

            MessageBox.Show("Call stopped.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Chọn chế độ: server hoặc client
            var result = MessageBox.Show("Are you the server?", "Select Role", MessageBoxButtons.YesNo);
            isServer = (result == DialogResult.Yes);

            if (!isServer)
            {
                txtServerIP.Enabled = true;
            }
            else
            {
                txtServerIP.Enabled = false;
            }
        }
    }
}
