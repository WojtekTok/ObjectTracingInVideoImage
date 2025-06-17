using System.Drawing;
using System.Text.Json;

namespace ObjectTracingInVideoImage.Core.ApiClients
{
    internal class SiamMaskApiClient : ISiamMaskApiClient
    {
        private readonly string _serverUrl = "http://127.0.0.1:5000";
        private readonly HttpClient _httpClient;

        public SiamMaskApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAsync(byte[] imageBytes, Rectangle selection)
        {
            var form = new MultipartFormDataContent
            {
                { new ByteArrayContent(imageBytes), "image", "frame.jpg" },
                { new StringContent($"{selection.X},{selection.Y},{selection.Width},{selection.Height}"), "bbox" }
            };

            var response = await _httpClient.PostAsync($"{_serverUrl}/init", form);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Rectangle?> TrackAsync(byte[] imageBytes)
        {
            var form = new MultipartFormDataContent
            {
                { new ByteArrayContent(imageBytes), "image", "frame.jpg" }
            };

            var response = await _httpClient.PostAsync($"{_serverUrl}/track", form);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
                return null;
            float[] arr = JsonSerializer.Deserialize<float[]>(content);
            if (arr == null || arr.Length != 4)
                return null;

            return Rectangle.Round(new RectangleF(arr[0], arr[1], arr[2], arr[3]));
        }
    }
}
