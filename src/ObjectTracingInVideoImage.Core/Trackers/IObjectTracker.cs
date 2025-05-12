using Emgu.CV;
using System.Drawing;

namespace ObjectTracingInVideoImage.Core.Trackers
{
    public interface IObjectTracker : IDisposable
    {
        void Initialize(Mat initialFrame, Rectangle selection);
        Rectangle? Track(Mat frame);
    }
}
