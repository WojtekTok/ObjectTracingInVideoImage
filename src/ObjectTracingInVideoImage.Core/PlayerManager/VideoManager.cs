using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Diagnostics;

namespace ObjectTracingInVideoImage.Core.PlayerManager
{
    public class VideoManager : IPlayerManager
    {
        private VideoCapture? _videoCapture;
        private CancellationTokenSource? _cancellationTokenSource;

        public bool IsPlaying { get; set; } = false;
        public bool IsPaused { get; set; } = false;
        public double Fps { get; set; }

        public bool LoadVideo(string filePath)
        {
            _videoCapture = new VideoCapture(filePath);
            if (_videoCapture.IsOpened)
            {
                Fps = _videoCapture.Get(CapProp.Fps);
                IsPlaying = false;
                IsPaused = false;
                _cancellationTokenSource?.Cancel();
                return true;
            }
            return false;
        }

        public void Pause() => IsPaused = true;

        public void Resume() => IsPaused = false;

        public void Stop()
        {
            IsPaused = false;
            IsPlaying = false;
            _cancellationTokenSource?.Cancel();
        }

        public async Task StartVideoAsync(Func<Mat, Task> onFrame)
        {
            if (_videoCapture == null)
                return;

            IsPlaying = true;
            IsPaused = false;
            _cancellationTokenSource = new CancellationTokenSource();

            var token = _cancellationTokenSource.Token;
            var stopwatch = Stopwatch.StartNew();
            var nextFrameTime = stopwatch.Elapsed;

            while (IsPlaying && !token.IsCancellationRequested)
            {
                if (IsPaused)
                {
                    stopwatch.Stop();
                    await Task.Delay(50);
                    continue;
                }
                stopwatch.Start();

                Mat frame = new Mat();
                _videoCapture.Read(frame);

                if (frame == null || frame.IsEmpty)
                {
                    IsPlaying = false;
                    break;
                }

                await onFrame(frame.Clone());

                var frameInterval = TimeSpan.FromMilliseconds(1000.0 / Fps);
                nextFrameTime += frameInterval;
                var delay = nextFrameTime - stopwatch.Elapsed;
                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay);
            }
        }

        public Mat? GetFirstFrame()
        {
            if (_videoCapture != null)
            {
                _videoCapture.Set(CapProp.PosFrames, 0);
                return _videoCapture.QueryFrame();
            }
            return null;
        }

    }
}
