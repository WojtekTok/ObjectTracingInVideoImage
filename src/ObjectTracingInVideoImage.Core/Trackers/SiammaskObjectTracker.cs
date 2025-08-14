using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using Emgu.CV;
using ObjectTracingInVideoImage.Core.ApiClients;

namespace ObjectTracingInVideoImage.Core.Trackers
{
    public class SiammaskObjectTracker : IObjectTracker
    {
        private readonly ISiamMaskApiClient _httpClient;
        private bool _initialized = false;

        public SiammaskObjectTracker()
        {
            _httpClient = new SiamMaskApiClient();
        }

        public void Initialize(Mat initialFrame, Rectangle selection)
        {
            byte[] imageBytes = MatToJpegBytes(initialFrame);

            var response = _httpClient.InitializeAsync(imageBytes, selection);
            _initialized = true;
        }

        public Rectangle? Track(Mat frame)
        {
            if (!_initialized)
                throw new InvalidOperationException("Tracker not initialized.");

            byte[] imageBytes = MatToJpegBytes(frame);
            var response = _httpClient.TrackAsync(imageBytes);

            return response.Result;
        }

        private byte[] MatToJpegBytes(Mat frame)
        {
            var arr = CvInvoke.Imencode(".jpg", frame);
            return (byte[])arr;
        }

        public void Dispose() { }
    }
}
