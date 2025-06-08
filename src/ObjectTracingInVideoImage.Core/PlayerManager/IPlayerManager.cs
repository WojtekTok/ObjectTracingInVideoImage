using Emgu.CV;

namespace ObjectTracingInVideoImage.Core.PlayerManager
{
    public interface IPlayerManager
    {
        bool IsPlaying { get; set; }
        bool IsPaused { get; set; }
        double Fps { get; set; }

        bool LoadVideo(string filePath);

        void Pause();

        void Resume();

        public void Stop();

        Task StartVideoAsync(Func<Mat, Task> onFrame);

        Mat? GetFirstFrame();
    }
}
