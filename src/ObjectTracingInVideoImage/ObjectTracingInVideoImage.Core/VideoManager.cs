using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Diagnostics;

namespace ObjectTracingInVideoImage.Core
{
    public class VideoManager
    {
        private VideoCapture? _videoCapture;
        private bool _isPlaying = false;
        private Stopwatch _stopwatch = new Stopwatch();
        private long _lastFrameNumber = -1;

        public double Fps { get; set; }

        public bool LoadVideo(string filePath)
        {
            _videoCapture = new VideoCapture(filePath);
            if (_videoCapture.IsOpened)
            {
                Fps = _videoCapture.Get(CapProp.Fps);
                return true;
            }
            return false;
        }

        public void Play() =>  _isPlaying = true;
        public void Pause() => _isPlaying = false;

        // Gets the next frame from the video
        public Mat? GetNextFrame()
        {
            if (_videoCapture == null || !_isPlaying)
                return null;

            Mat frame = new Mat();
            _videoCapture.Read(frame);

            if (frame.IsEmpty)
                return null;

            return frame;
        }
    }
}
