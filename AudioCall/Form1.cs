using NAudio.Wave; // Thư viện hỗ trợ xử lý âm thanh
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
        private TcpClient client; // Đối tượng để thiết lập kết nối TCP cho client
        private TcpListener server; // Đối tượng để thiết lập server lắng nghe các kết nối từ client
        private NetworkStream stream; // Dòng dữ liệu truyền qua lại giữa server và client
        private bool isServer; // Kiểm tra chế độ server hay client
        private bool isConnected = false; // Kiểm tra trạng thái kết nối

        private WaveOut waveOut; // Đối tượng phát âm thanh
        private BufferedWaveProvider waveProvider; // Bộ đệm cung cấp dữ liệu âm thanh để phát
        private WaveInEvent waveIn; // Đối tượng thu âm thanh từ microphone
        private WaveFileWriter waveFileWriter; // Ghi dữ liệu âm thanh vào file

        private bool isRecording = false; // Trạng thái kiểm tra có đang ghi âm hay không

        public Form1()
        {
            InitializeComponent(); // Khởi tạo các thành phần giao diện
            InitializeAudioControls(); // Gọi hàm khởi tạo các điều khiển liên quan đến âm thanh
        }

        private void InitializeAudioControls()
        {
            // Trạng thái default của button
            btnTalk.Enabled = false;
            btnEndCall.Enabled = false;
            btnConnect.BackColor = Color.LightGreen;
            btnEndCall.BackColor = Color.LightPink;
            btnTalk.BackColor = Color.LightGray;

            // Tạo thanh tăng giảm âm lượng
            volumeSlider.Minimum = 0;
            volumeSlider.Maximum = 100;
            volumeSlider.Value = 50;  
            volumeSlider.Enabled = true;

            // Gán sự kiện khi thay đổi giá trị của thanh trượt âm lượng
            volumeSlider.Scroll += new EventHandler(VolumeSlider_Scroll);

            rbServer.Checked = true; // Mặc định là chế độ server
            isServer = true;
        }

        // Hàm xử lý sự kiện thay đổi âm lượng
        private void VolumeSlider_Scroll(object sender, EventArgs e)
        {
            if (waveOut != null)
            {
                waveOut.Volume = volumeSlider.Value / 100f;  // Điều chỉnh âm lượng theo giá trị
                statusLabel.Text = $"Volume: {volumeSlider.Value}%"; // Hiển thị mức âm lượng
            }
        }

        // Xử lý khi người dùng chọn chế độ server
        private void rbServer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbServer.Checked)
            {
                isServer = true;
                txtServerIP.Enabled = false; // Không cần nhập IP khi là server
            }
        }

        // Xử lý khi người dùng chọn chế độ client
        private void rbClient_CheckedChanged(object sender, EventArgs e)
        {
            if (rbClient.Checked)
            {
                isServer = false;
                txtServerIP.Enabled = true; // Client cần nhập IP của server
            }
        }

        // Khởi động server và lắng nghe kết nối từ client
        private async Task StartServerAsync()
        {
            server = new TcpListener(IPAddress.Any, 5000); // Khởi tạo server lắng nghe ở cổng 5000
            server.Start(); // Bắt đầu lắng nghe kết nối
            statusLabel.Text = "Server started..."; // Cập nhật trạng thái server
            client = await server.AcceptTcpClientAsync(); // Chấp nhận kết nối từ client
            stream = client.GetStream(); // Lấy luồng dữ liệu từ client
            isConnected = true; // Đánh dấu đã kết nối
            statusLabel.Text = "Client connected!"; // Cập nhật trạng thái kết nối thành công
            StartListeningForAudio(); // Bắt đầu lắng nghe và phát âm thanh
        }

        // Khởi tạo client và kết nối với server
        private async Task StartClientAsync()
        {
            client = new TcpClient(); // Tạo đối tượng client
            await client.ConnectAsync(txtServerIP.Text, 5000); // Kết nối đến IP và cổng của server
            stream = client.GetStream(); // Lấy luồng dữ liệu từ server
            isConnected = true; // Đánh dấu đã kết nối
            statusLabel.Text = "Connected to server!"; // Cập nhật trạng thái kết nối thành công
            StartListeningForAudio(); // Bắt đầu lắng nghe và phát âm thanh
        }

        // Xử lý khi người dùng nhấn nút "Connect"
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                btnConnect.Enabled = false;  // Vô hiệu hóa nút khi đang kết nối
                statusLabel.Text = "Connecting...";

                try
                {
                    if (isServer)
                        await StartServerAsync(); // Nếu là server thì khởi động server
                    else
                        await StartClientAsync(); // Nếu là client thì kết nối đến server

                    // Sau khi kết nối thành công, bật các nút điều khiển và thanh trượt âm lượng
                    btnTalk.Enabled = true;
                    btnEndCall.Enabled = true;
                    volumeSlider.Enabled = true;
                    statusLabel.Text = "Connected. Ready to talk."; // Cập nhật trạng thái
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Error: {ex.Message}"; // Thông báo lỗi nếu có
                    btnConnect.Enabled = true; // Cho phép thử lại kết nối
                }
            }
        }

        // Xử lý khi người dùng nhấn nút "End Call"
        private void btnEndCall_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Call ended."; // Cập nhật trạng thái cuộc gọi kết thúc
            isConnected = false;
            btnTalk.Enabled = false;
            btnEndCall.Enabled = false;
            volumeSlider.Enabled = false; // Vô hiệu hóa thanh trượt âm lượng sau khi kết thúc cuộc gọi

            if (waveIn != null)
            {
                waveIn.StopRecording(); // Dừng ghi âm
                waveIn.Dispose(); // Giải phóng tài nguyên
                waveIn = null;
            }

            if (waveFileWriter != null)
            {
                waveFileWriter.Dispose(); // Giải phóng file ghi âm
                waveFileWriter = null;
            }

            if (stream != null)
            {
                stream.Close(); // Đóng luồng dữ liệu
                stream = null;
            }

            if (client != null)
            {
                client.Close(); // Đóng kết nối client
                client = null;
            }

            if (server != null)
            {
                server.Stop(); // Dừng server
                server = null;
            }
        }

        // Bắt đầu lắng nghe và phát âm thanh từ kết nối TCP
        private void StartListeningForAudio()
        {
            waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1)); // Khởi tạo bộ đệm âm thanh
            waveOut = new WaveOut();
            waveOut.Volume = volumeSlider.Value / 100f; // Đặt âm lượng ban đầu
            waveOut.Init(waveProvider); // Khởi tạo đối tượng phát âm thanh với dữ liệu từ bộ đệm
            waveOut.Play(); // Bắt đầu phát âm thanh

            Task.Run(() =>
            {
                byte[] buffer = new byte[1024];
                while (isConnected)
                {
                    try
                    {
                        // Đọc dữ liệu âm thanh từ luồng
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            waveProvider.AddSamples(buffer, 0, bytesRead); // Thêm dữ liệu vào bộ đệm và phát lại
                        }
                    }
                    catch (Exception ex)
                    {
                        statusLabel.Text = $"Error while receiving audio: {ex.Message}"; // Thông báo lỗi nếu có
                        break;
                    }
                }
            });
        }

        // Xử lý khi người dùng nhấn nút "Talk"
        private void btnTalk_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                waveIn = new WaveInEvent(); // Khởi tạo đối tượng thu âm thanh
                waveIn.WaveFormat = new WaveFormat(44100, 1); // Thiết lập định dạng âm thanh
                waveIn.DataAvailable += OnDataAvailable; // Gán sự kiện khi có dữ liệu âm thanh

                // Tạo file ghi âm với tên file dựa theo thời gian hiện tại
                string fileName = $"recordedAudio_{DateTime.Now:ddMMyyyy_HHmmss}.wav";
                waveFileWriter = new WaveFileWriter(fileName, waveIn.WaveFormat); // Tạo đối tượng ghi file

                waveIn.StartRecording(); // Bắt đầu ghi âm
                isRecording = true; // Đánh dấu đang ghi âm

                statusLabel.Text = "Talking..."; 
                btnTalk.Text = "Stop Talking"; 
            }
            else
            {
                waveIn.StopRecording(); // Dừng ghi âm
                waveIn.Dispose(); // Giải phóng tài nguyên
                waveIn = null;

                if (waveFileWriter != null)
                {
                    waveFileWriter.Dispose(); // Đóng file ghi âm
                    waveFileWriter = null;
                }

                isRecording = false; // Đánh dấu đã dừng ghi âm

                statusLabel.Text = "Stopped talking."; 
                btnTalk.Text = "Talk"; 
            }
        }

        // Sự kiện khi có dữ liệu âm thanh mới
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (stream != null && stream.CanWrite)
                {
                    stream.Write(e.Buffer, 0, e.BytesRecorded); // Gửi dữ liệu âm thanh qua mạng
                }

                if (waveFileWriter != null)
                {
                    waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded); // Ghi dữ liệu vào file
                    waveFileWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Lỗi khi gửi/ nhận file audio: {ex.Message}"; // Thông báo lỗi nếu có
            }
        }
    }
}
