using System.Drawing;
using Emgu.CV;
using ObjectTracingInVideoImage.Core.KalmanFilter;

namespace ObjectTracingInVideoImage.Core.Trackers
{
    public class Test : IObjectTracker, IKalmanTracker
    {
        private TrackerKCF? _tracker;
        private readonly IKalmanFilter2D _kalman;
        public KalmanData _kalmanData = new();

        public Test()
        {
            _kalman = new KalmanFilter2D();
        }

        public void Initialize(Mat initialFrame, Rectangle selection)
        {
            _tracker?.Dispose();
            _tracker = new TrackerKCF();
            _tracker.Init(initialFrame, selection);

            PointF center = new PointF(selection.X + selection.Width / 2f, selection.Y + selection.Height / 2f);
            _kalman.Init(center);
            _kalmanData.PredictedPath.Clear();
            _kalmanData.ObservedPath.Clear();
        }

        public Rectangle? Track(Mat frame)
        {
            if (_tracker is null)
                return null;

            Rectangle trackedRect = new Rectangle();
            bool success = _tracker.Update(frame, out trackedRect);

            PointF predicted = _kalman.Predict();
            _kalmanData.PredictedPath.Add(Point.Round(predicted));

            if (success)
            {
                PointF observed = new PointF(trackedRect.X + trackedRect.Width / 2f, trackedRect.Y + trackedRect.Height / 2f);
                _kalman.Correct(observed);
                _kalmanData.ObservedPath.Add(Point.Round(observed));
            }

            return success ? trackedRect : null;
        }

        public KalmanData GetKalmanData()
        {
            return _kalmanData;
        }

        public void Dispose()
        {
            _tracker?.Dispose();
            _tracker = null;
        }
    }
}
