using System.Diagnostics;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace ObjectTracingInVideoImage.Core.PlayerManager
{
    public class ImageSequenceManager : IPlayerManager
    {
        private string? _directoryPath;
        private CancellationTokenSource? _cancellationTokenSource;
        private int _currentFrameIndex;
        private int _frameCount;
        public double Fps { get; set; } = 30;
        public bool IsPlaying { get; set; }
        public bool IsPaused { get; set; }

        public bool LoadVideo(string directoryPath)
        {
            if (!File.Exists(directoryPath)) return false;
            _directoryPath = Path.GetDirectoryName(directoryPath);

            var files = Directory.GetFiles(_directoryPath!, "*.jpg")
                .Select(f => new {
                    Path = f,
                    Name = Path.GetFileNameWithoutExtension(f)
                })
                .Where(x => int.TryParse(x.Name, out _))
                .ToList();

            if (!files.Any())
                return false;

            _frameCount = files.Max(x => int.Parse(x.Name));
            _currentFrameIndex = 1;
            return true;
        }

        public void Pause() => IsPaused = true;
        public void Resume() => IsPaused = false;
        public void Stop()
        {
            IsPlaying = false;
            IsPaused = false;
            _cancellationTokenSource?.Cancel();
        }

        public async Task StartVideoAsync(Func<Mat, Task> onFrame)
        {
            if (_frameCount == 0) return;

            IsPlaying = true;
            IsPaused = false;
            _cancellationTokenSource = new CancellationTokenSource();

            var token = _cancellationTokenSource.Token;
            var stopwatch = Stopwatch.StartNew();
            var nextFrameTime = stopwatch.Elapsed;

            while (IsPlaying && _currentFrameIndex <= _frameCount && !token.IsCancellationRequested)
            {
                if (IsPaused)
                {
                    stopwatch.Stop();
                    await Task.Delay(50);
                    continue;
                }

                stopwatch.Start();
                string filePath = Path.Combine(_directoryPath!, $"{_currentFrameIndex:D8}.jpg");
                _currentFrameIndex++;
                if (!File.Exists(filePath)) continue;

                using var frame = CvInvoke.Imread(filePath, ImreadModes.Color);
                if (frame == null || frame.IsEmpty) continue;

                await onFrame(frame.Clone());

                var frameInterval = TimeSpan.FromMilliseconds(1000.0 / Fps);
                nextFrameTime += frameInterval;
                var delay = nextFrameTime - stopwatch.Elapsed;
                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay);
            }

            IsPlaying = false;
        }

        public Mat? GetFirstFrame()
        {
            var files = Directory.GetFiles(_directoryPath, "*.jpg")
                .Select(f => new {
                    Path = f,
                    Name = Path.GetFileNameWithoutExtension(f)
                })
                .Where(x => int.TryParse(x.Name, out _))
                .ToList();

            if (!files.Any())
                return null;

            var minNum = files.Min(x => int.Parse(x.Name));
            var filePath = Path.Combine(_directoryPath, $"{minNum:D8}.jpg");
            if (File.Exists(filePath))
                return CvInvoke.Imread(filePath, Emgu.CV.CvEnum.ImreadModes.Color);

            return null;
        }
    }

}
