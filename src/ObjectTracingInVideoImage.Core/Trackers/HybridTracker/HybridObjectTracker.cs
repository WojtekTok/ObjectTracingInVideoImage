using ObjectTracingInVideoImage.Core.KalmanFilter;
using ObjectTracingInVideoImage.Core.ApiClients;
using System.Drawing;
using Emgu.CV;

namespace ObjectTracingInVideoImage.Core.Trackers.HybridTracker
{
    public class HybridObjectTracker : IObjectTracker, IKalmanTracker
    {
        private readonly IObjectTracker _classicTracker;
        private readonly SiamMaskApiClient _siamApiClient;
        private readonly IKalmanFilter2D _kalman;
        private readonly KalmanData _kalmanData;
        private int _frameCountSinceSiam = 0;
        private int _consecutiveSiamFailures = 0;
        private PointF? _lastValidDetection = null;

        public HybridObjectTracker(IObjectTracker classicTracker)
        {
            _classicTracker = classicTracker ?? throw new ArgumentNullException(nameof(classicTracker));
            _siamApiClient = new SiamMaskApiClient();
            _kalman = new KalmanFilter2D();
            _kalmanData = new KalmanData();
        }

        public void Initialize(Mat initialFrame, Rectangle selection)
        {
            _classicTracker.Initialize(initialFrame, selection);
            byte[] imageBytes = CvInvoke.Imencode(".jpg", initialFrame).ToArray();
            _siamApiClient.InitializeAsync(imageBytes, selection);
            _siamApiClient.UpdateThresholdAsync(0.88f);

            PointF center = new PointF(selection.X + selection.Width / 2f, selection.Y + selection.Height / 2f);
            _kalman.Init(center);
            _kalmanData.Clear();
            _kalmanData.AddPredictedPoint(center);
            _kalmanData.AddObservedPoint(center);
            _frameCountSinceSiam = 0;
            _consecutiveSiamFailures = 0;
            _lastValidDetection = center;
        }

        public Rectangle? Track(Mat frame)
        {
            _frameCountSinceSiam++;
            byte[] imageBytes = CvInvoke.Imencode(".jpg", frame).ToArray();

            PointF predictedCenter = _kalman.Predict();
            predictedCenter = ClampToFrame(predictedCenter, frame);
            _kalmanData.AddPredictedPoint(predictedCenter);

            if (_frameCountSinceSiam >= 10)
            {
                PointF roi = predictedCenter;

                if (_consecutiveSiamFailures >= 15 && _consecutiveSiamFailures % 2 == 0 && _lastValidDetection.HasValue)
                {
                    roi = _lastValidDetection.Value;
                }

                _siamApiClient.UpdateRoiAsync(roi).Wait();
                var siamResult = _siamApiClient.TrackAsync(imageBytes).Result;

                if (siamResult.HasValue)
                {
                    PointF observed = new PointF(
                        siamResult.Value.X + siamResult.Value.Width / 2f,
                        siamResult.Value.Y + siamResult.Value.Height / 2f);

                    observed = ClampToFrame(observed, frame);
                    _kalmanData.AddPredictedPoint(observed);
                    _kalmanData.AddObservedPoint(observed);

                    _classicTracker.Initialize(frame, siamResult.Value);
                    _frameCountSinceSiam = 0;
                    _consecutiveSiamFailures = 0;
                    _lastValidDetection = observed;
                    return siamResult;
                }
                else
                {
                    _consecutiveSiamFailures++;
                    return null;
                }
            }

            Rectangle? trackedRect = _classicTracker.Track(frame);
            if (trackedRect.HasValue)
            {
                PointF observed = new PointF(
                    trackedRect.Value.X + trackedRect.Value.Width / 2f,
                    trackedRect.Value.Y + trackedRect.Value.Height / 2f);

                observed = ClampToFrame(observed, frame);
                _kalman.Correct(observed);
                _kalmanData.AddObservedPoint(observed);
                _lastValidDetection = observed;

                _consecutiveSiamFailures = 0;
                return trackedRect;
            }

            return null;
        }

        private PointF ClampToFrame(PointF point, Mat frame)
        {
            float clampedX = Math.Max(0, Math.Min(frame.Width - 1, point.X));
            float clampedY = Math.Max(0, Math.Min(frame.Height - 1, point.Y));
            return new PointF(clampedX, clampedY);
        }

        public KalmanData GetKalmanData()
        {
            return _kalmanData;
        }

        public void Dispose()
        {
            _classicTracker?.Dispose();
        }
    }
}
