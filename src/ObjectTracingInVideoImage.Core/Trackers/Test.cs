using Emgu.CV;
using System.Drawing;

namespace ObjectTracingInVideoImage.Core.Trackers
{
    public class Test : IObjectTracker
    {
        private TrackerKCF? _tracker;
        public void Initialize(Mat initialFrame, Rectangle selection)
        {
            _tracker?.Dispose();
            _tracker ??= new();
            _tracker.Init(initialFrame, selection);
        }

        public Rectangle? Track(Mat frame)
        {
            if (_tracker is null)
                return null;

            Rectangle trackedRect = new Rectangle();
            bool success = _tracker.Update(frame, out trackedRect);

            return success ? trackedRect : null;
        }
        public void Dispose()
        {
            _tracker?.Dispose();
            _tracker = null;
        }
    }
}
