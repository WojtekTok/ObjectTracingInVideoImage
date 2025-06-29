using ObjectTracingInVideoImage.Core.KalmanFilter;
using ObjectTracingInVideoImage.Core.ApiClients;
using System.Drawing;
using Emgu.CV;

namespace ObjectTracingInVideoImage.Core.Trackers
{
    public class HybridObjectTracker : IObjectTracker, IKalmanTracker
    {
        private readonly IObjectTracker _classicTracker;
        private readonly SiamMaskApiClient _siamApiClient;
        private readonly IKalmanFilter2D _kalman;
        private readonly KalmanData _kalmanData;
        private int _frameCountSinceSiam = 0;
        private bool _forceSiamNext = false;


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
            var initResponse = _siamApiClient.InitializeAsync(imageBytes, selection); // This has to be awaited somewhere probably
            var thresholdResponse = _siamApiClient.UpdateThresholdAsync(0.85f);
            PointF center = new PointF(selection.X + selection.Width / 2f, selection.Y + selection.Height / 2f);
            _kalman.Init(center);
            _kalmanData.Clear();
            _kalmanData.AddPredictedPoint(center);
            _kalmanData.AddObservedPoint(center);
            _frameCountSinceSiam = 0;
        }

        public Rectangle? Track(Mat frame)
        {
            _frameCountSinceSiam++;
            byte[] imageBytes = CvInvoke.Imencode(".jpg", frame).ToArray();

            if (_frameCountSinceSiam >= 10 || _forceSiamNext)
            {
                PointF predictedCenter = _kalman.Predict();
                _kalmanData.AddPredictedPoint(predictedCenter);

                _siamApiClient.UpdateRoiAsync(predictedCenter).Wait();

                var siamResult = _siamApiClient.TrackAsync(imageBytes).Result;
                if (siamResult.HasValue)
                {
                    PointF observed = new PointF(
                        siamResult.Value.X + siamResult.Value.Width / 2f,
                        siamResult.Value.Y + siamResult.Value.Height / 2f);

                    _kalman.Correct(observed);
                    _kalmanData.AddObservedPoint(observed);

                    _classicTracker.Initialize(frame, siamResult.Value);
                    _frameCountSinceSiam = 0;
                    return siamResult;
                }
                _forceSiamNext = true;
            }

            Rectangle? trackedRect = _classicTracker.Track(frame);
            if (trackedRect.HasValue)
            {
                PointF observed = new PointF(
                    trackedRect.Value.X + trackedRect.Value.Width / 2f,
                    trackedRect.Value.Y + trackedRect.Value.Height / 2f);
                PointF predicted = _kalman.Predict();

                _kalmanData.AddPredictedPoint(predicted);
                _kalman.Correct(observed);
                _kalmanData.AddObservedPoint(observed);

                return trackedRect;
            }
            else
            {
                PointF predictedCenter = _kalman.Predict();
                _kalmanData.AddPredictedPoint(predictedCenter);

                _siamApiClient.UpdateRoiAsync(predictedCenter).Wait();
                var siamResult = _siamApiClient.TrackAsync(imageBytes).Result;
                if (siamResult.HasValue)
                {
                    PointF observed = new PointF(
                        siamResult.Value.X + siamResult.Value.Width / 2f,
                        siamResult.Value.Y + siamResult.Value.Height / 2f);

                    _kalman.Correct(observed);
                    _kalmanData.AddObservedPoint(observed);

                    _classicTracker.Initialize(frame, siamResult.Value);
                    _frameCountSinceSiam = 0;
                    return siamResult;
                }
            }

            return null;
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
