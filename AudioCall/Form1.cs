using NAudio.Wave;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioCallApp
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private TcpListener server;
        private NetworkStream stream;
        private bool isServer;  // Dùng để kiểm tra xem người dùng là server hay client
        private bool isConnected = false;
        
        private WaveOut waveOut;
        private BufferedWaveProvider waveProvider;
        private WaveInEvent waveIn;


        private bool isMuted = true;
        public Form1()
        {
            InitializeComponent();
            InitializeAudioControls();
        }

        private void InitializeAudioControls()
        {
            btnTalk.Enabled = false;  // Giữ lại nếu sau này bạn muốn thêm chức năng thu âm
            btnEndCall.Enabled = false;
            btnConnect.BackColor = Color.LightGreen;
            btnEndCall.BackColor = Color.LightPink;
            btnTalk.BackColor = Color.LightGray;

            // Bật/tắt volume slider nếu có logic xử lý
            volumeSlider.Minimum = 0;
            volumeSlider.Maximum = 100;
            volumeSlider.Value = 50;
            volumeSlider.Enabled = false;

            // Radio buttons cho việc chọn server hay client
            rbServer.Checked = true;  // Default là server
            isServer = true;
        }

        // Xóa các phương thức trùng lặp và giữ lại định nghĩa duy nhất:
        private void rbServer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbServer.Checked)
            {
                isServer = true;
                txtServerIP.Enabled = false; // Server không cần nhập IP
            }
        }

        private void rbClient_CheckedChanged(object sender, EventArgs e)
        {
            if (rbClient.Checked)
            {
                isServer = false;
                txtServerIP.Enabled = true; // Client cần nhập IP của server
            }
        }

        private async Task StartServerAsync()
        {
            server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            statusLabel.Text = "Server started...";
            client = await server.AcceptTcpClientAsync();
            stream = client.GetStream();
            isConnected = true;
            statusLabel.Text = "Client connected!";
            StartListeningForAudio();  // Bắt đầu lắng nghe âm thanh
        }

        private async Task StartClientAsync()
        {
            client = new TcpClient();
            await client.ConnectAsync(txtServerIP.Text, 5000); // IP và port của server
            stream = client.GetStream();
            isConnected = true;
            statusLabel.Text = "Connected to server!";
            StartListeningForAudio();  // Bắt đầu lắng nghe âm thanh
        }

        private void SetupSuccessfulConnection()
        {
            isConnected = true;
            btnConnect.Enabled = false;
            btnEndCall.Enabled = true;
            btnTalk.Enabled = true;
            volumeSlider.Enabled = true;
            txtServerIP.Enabled = false;

            InitializeAudioDevices();
            StartReceivingAudio();
        }

        private void InitializeAudioDevices()
        {
            try
            {
                // Setup audio input với WaveInEvent
                waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(44100, 1) // Thiết lập định dạng âm thanh
                };
                waveIn.DataAvailable += WaveIn_DataAvailable;

                // Setup audio output
                waveOut = new WaveOut();
                waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
                waveOut.Init(waveProvider);
                waveOut.Volume = volumeSlider.Value / 100f;
                waveOut.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing audio devices: {ex.Message}", "Audio Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (stream != null && stream.CanWrite && !isMuted)
                {
                    stream.Write(e.Buffer, 0, e.BytesRecorded);
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    statusLabel.Text = $"Audio sending error: {ex.Message}";
                });
            }
        }


        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                btnConnect.Enabled = false;  // Vô hiệu hóa nút Connect khi đang kết nối
                statusLabel.Text = "Connecting...";

                try
                {
                    if (isServer)
                        await StartServerAsync();  // Bắt đầu server nếu chọn là server
                    else
                        await StartClientAsync();  // Kết nối với server nếu chọn là client

                    // Sau khi kết nối thành công, bật nút Talk và End Call
                    btnTalk.Enabled = true;
                    btnEndCall.Enabled = true;
                    statusLabel.Text = "Connected. Ready to talk.";  // Cập nhật trạng thái
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Error: {ex.Message}";
                    btnConnect.Enabled = true;  // Nếu có lỗi, cho phép thử lại kết nối
                }
            }
        }


        // Định nghĩa các sự kiện Click cho các nút để tránh lỗi:
        private void btnEndCall_Click(object sender, EventArgs e)
        {
            // Logic để kết thúc cuộc gọi
            statusLabel.Text = "Call ended.";
            isConnected = false;
            btnTalk.Enabled = false;
            btnEndCall.Enabled = false;
            waveIn?.StopRecording(); // Tắt ghi âm
            waveIn?.Dispose();       // Giải phóng tài nguyên
        }
        private void StartListeningForAudio()
        {
            waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
            waveOut = new WaveOut();
            waveOut.Init(waveProvider);
            waveOut.Play();

            Task.Run(() =>
            {
                byte[] buffer = new byte[1024];
                while (isConnected)
                {
                    try
                    {
                        // Đọc dữ liệu âm thanh từ stream và phát lại
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            waveProvider.AddSamples(buffer, 0, bytesRead);
                        }
                    }
                    catch (Exception ex)
                    {
                        statusLabel.Text = $"Error while receiving audio: {ex.Message}";
                        break;
                    }
                }
            });
        }


        private void btnTalk_Click(object sender, EventArgs e)
        {
            if (waveIn == null)
            {
                waveIn = new WaveInEvent();
                waveIn.WaveFormat = new WaveFormat(44100, 1); // 44.1kHz, Mono
                waveIn.DataAvailable += OnDataAvailable;  // Sự kiện khi có dữ liệu âm thanh
                waveIn.StartRecording();  // Bắt đầu thu âm

                statusLabel.Text = "Talking...";  // Cập nhật trạng thái
            }

            if (waveIn != null && !isMuted)
            {
                waveIn.StartRecording();
                statusLabel.Text = "Talking...";  // Cập nhật trạng thái
            }
        }

        // Sự kiện khi có dữ liệu âm thanh (khi đang thu âm)
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                // Gửi dữ liệu âm thanh qua mạng (TCP)
                if (stream != null && stream.CanWrite)
                {
                    stream.Write(e.Buffer, 0, e.BytesRecorded);
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Error while sending audio: {ex.Message}";
            }
        }

    }
}
