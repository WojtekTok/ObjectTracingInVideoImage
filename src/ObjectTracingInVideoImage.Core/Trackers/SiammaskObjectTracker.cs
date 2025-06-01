using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using Emgu.CV;

namespace ObjectTracingInVideoImage.Core.Trackers
{
    public class SiammaskObjectTracker : IObjectTracker
    {
        private readonly string _serverUrl;
        private readonly HttpClient _httpClient;
        private bool _initialized = false;

        public SiammaskObjectTracker(string serverUrl = "http://127.0.0.1:5000")
        {
            _serverUrl = serverUrl.TrimEnd('/');
            _httpClient = new HttpClient();
        }

        public void Initialize(Mat initialFrame, Rectangle selection)
        {
            var sw = Stopwatch.StartNew();
            byte[] imageBytes = MatToJpegBytes(initialFrame);
            sw.Stop();
            File.AppendAllText("timelog.txt", $"[C#] JPEG encode: {sw.ElapsedMilliseconds} ms\n");

            sw.Restart();
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(imageBytes), "image", "frame.jpg");
            form.Add(new StringContent($"{selection.X},{selection.Y},{selection.Width},{selection.Height}"), "bbox");
            sw.Stop();
            File.AppendAllText("timelog.txt", $"[C#] HTTP round-trip: {sw.ElapsedMilliseconds} ms\n");

            sw.Restart();
            var response = _httpClient.PostAsync($"{_serverUrl}/init", form).Result;
            response.EnsureSuccessStatusCode();
            sw.Stop();
            File.AppendAllText("timelog.txt", $"[C#] JSON parse: {sw.ElapsedMilliseconds} ms\n");
            _initialized = true;
        }

        public Rectangle? Track(Mat frame)
        {
            if (!_initialized)
                throw new InvalidOperationException("Tracker not initialized. Call Initialize() first.");

            byte[] imageBytes = MatToJpegBytes(frame);

            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(imageBytes), "image", "frame.jpg");

            var response = _httpClient.PostAsync($"{_serverUrl}/track", form).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            float[] arr = JsonSerializer.Deserialize<float[]>(content);
            Rectangle bbox = Rectangle.Round(new RectangleF(arr[0], arr[1], arr[2], arr[3]));
            return bbox;
        }

        private byte[] MatToJpegBytes(Mat frame)
        {
            var arr = CvInvoke.Imencode(".jpg", frame);
            return (byte[])arr;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
