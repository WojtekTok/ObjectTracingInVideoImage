using ObjectTracingInVideoImage.Core.KalmanFilter;
using ObjectTracingInVideoImage.Core.ApiClients;
using System.Drawing;
using Emgu.CV;
using ObjectTracingInVideoImage.Core.Enums;
using ObjectTracingInVideoImage.Core.Testing;
using ObjectTracingInVideoImage.Core.Trackers.ClassicTrackers;

namespace ObjectTracingInVideoImage.Core.Trackers.HybridTracker
{
    public class HybridObjectTrackerv2 : IObjectTracker, IKalmanTracker
    {
        private const int FramesEverySiamTrack = 7;
        private const int MaxSiamFailures = 15;
        private const float SiamThreshold = 0.88f;
        private const float KcfVelocityThreshold = 1.5f;
        private const int KcfSwitchFrames = 15;
        private const float IoUThreshold = 0.75f;

        private readonly KcfObjectTracker _kcfTracker;
        private readonly CsrtObjectTracker _csrtTracker;
        private readonly MilObjectTracker _milTracker;
        private readonly SiamMaskApiClient _siamApiClient;
        private readonly IKalmanFilter2D _kalman;
        private readonly KalmanData _kalmanData;
        private int _frameCountSinceSiam = 0;
        private int _consecutiveSiamFailures = 0;
        private PointF? _lastValidDetection = null;
        private TrackerType _currentTrackerType;
        private bool _wasLastTrackingSuccessful = false;
        private int _framesCountSinceKcf = 0;

        public HybridObjectTrackerv2()
        {
            _siamApiClient = new SiamMaskApiClient();
            _kalman = new KalmanFilter2D();
            _kalmanData = new KalmanData();
            _kcfTracker = new KcfObjectTracker();
            _csrtTracker = new CsrtObjectTracker();
            _milTracker = new MilObjectTracker();
            _currentTrackerType = TrackerType.CSRT;
        }

        public void Initialize(Mat initialFrame, Rectangle selection)
        {
            InitializeClassicTracker(initialFrame, selection);

            byte[] imageBytes = CvInvoke.Imencode(".jpg", initialFrame).ToArray();
            _siamApiClient.InitializeAsync(imageBytes, selection);
            _siamApiClient.UpdateThresholdAsync(SiamThreshold);

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
            _framesCountSinceKcf++;

            ChooseTracker();

            PointF predictedCenter = _kalman.Predict();
            predictedCenter = ClampToFrame(predictedCenter, frame);
            _kalmanData.AddPredictedPoint(predictedCenter);

            if (_frameCountSinceSiam >= FramesEverySiamTrack || !_wasLastTrackingSuccessful)
            {
                PointF roi = predictedCenter;

                if (_consecutiveSiamFailures >= MaxSiamFailures && _consecutiveSiamFailures % 2 == 0 && _lastValidDetection.HasValue)
                {
                    roi = _lastValidDetection.Value;
                }

                _siamApiClient.UpdateRoiAsync(roi).Wait();
                byte[] imageBytes = CvInvoke.Imencode(".jpg", frame).ToArray();
                var siamResult = _siamApiClient.TrackAsync(imageBytes).Result;
                _frameCountSinceSiam = 0;

                if (siamResult.HasValue)
                {
                    PointF observed = new PointF(
                        siamResult.Value.X + siamResult.Value.Width / 2f,
                        siamResult.Value.Y + siamResult.Value.Height / 2f);

                    observed = ClampToFrame(observed, frame);
                    _kalmanData.AddPredictedPoint(observed);
                    _kalmanData.AddObservedPoint(observed);

                    var currentClassicResult = TrackClassicTracker(frame);
                    if (currentClassicResult.HasValue)
                    {
                        double iou = TrackingEvaluator.CalculateIoU(currentClassicResult.Value, siamResult.Value);
                        if (iou < IoUThreshold)
                        {
                            _currentTrackerType = _currentTrackerType == TrackerType.CSRT ? TrackerType.MIL : TrackerType.CSRT;
                            InitializeClassicTracker(frame, siamResult.Value);
                        }
                    }
                    else
                    {
                        InitializeClassicTracker(frame, siamResult.Value);
                    }

                    _consecutiveSiamFailures = 0;
                    _lastValidDetection = observed;
                    _wasLastTrackingSuccessful = true;
                    return siamResult;
                }
                else
                {
                    _consecutiveSiamFailures++;
                    _wasLastTrackingSuccessful = false;
                    return null;
                }
            }

            Rectangle? trackedRect = TrackClassicTracker(frame);
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
                _wasLastTrackingSuccessful = true;
                return trackedRect;
            }

            _wasLastTrackingSuccessful = false;
            return null;
        }

        public KalmanData GetKalmanData()
        {
            return _kalmanData;
        }

        public void Dispose()
        {
            _kcfTracker?.Dispose();
            _csrtTracker?.Dispose();
            _milTracker?.Dispose();
        }

        private PointF ClampToFrame(PointF point, Mat frame)
        {
            float clampedX = Math.Max(0, Math.Min(frame.Width - 1, point.X));
            float clampedY = Math.Max(0, Math.Min(frame.Height - 1, point.Y));
            return new PointF(clampedX, clampedY);
        }

        private void InitializeClassicTracker(Mat initialFrame, Rectangle selection)
        {
            switch (_currentTrackerType)
            {
                case TrackerType.KCF:
                    _kcfTracker.Initialize(initialFrame, selection);
                    break;
                case TrackerType.CSRT:
                    _csrtTracker.Initialize(initialFrame, selection);
                    break;
                case TrackerType.MIL:
                    _milTracker.Initialize(initialFrame, selection);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported tracker type.");
            }
        }

        private Rectangle? TrackClassicTracker(Mat frame)
        {
            switch (_currentTrackerType)
            {
                case TrackerType.KCF:
                    return _kcfTracker.Track(frame);
                case TrackerType.CSRT:
                    return _csrtTracker.Track(frame);
                case TrackerType.MIL:
                    return _milTracker.Track(frame);
                default:
                    throw new InvalidOperationException("Unsupported tracker type.");
            }
        }

        private void ChooseTracker()
        {
            if (_currentTrackerType == TrackerType.KCF && !_wasLastTrackingSuccessful)
            {
                _framesCountSinceKcf = 0;
                _currentTrackerType = TrackerType.CSRT;
                return;
            }
            var velocity = VelocityEvaluator.ComputeVelocity(_kalmanData.ObservedPath);

            if (velocity < KcfVelocityThreshold && _framesCountSinceKcf > KcfSwitchFrames)
            {
                _currentTrackerType = TrackerType.KCF;
                return;
            }

            if (_currentTrackerType == TrackerType.CSRT || _currentTrackerType == TrackerType.MIL)
                return;
        }
    }
}
