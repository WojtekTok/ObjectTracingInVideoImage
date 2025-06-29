using System.Drawing;

namespace ObjectTracingInVideoImage.Core.ApiClients
{
    public interface ISiamMaskApiClient
    {
        public Task InitializeAsync(byte[] imageBytes, Rectangle selection);

        public Task<Rectangle?> TrackAsync(byte[] imageBytes);

        public Task UpdateRoiAsync(PointF targetPos);

        public Task UpdateThresholdAsync(float pscoreThreshold);
    }
}
