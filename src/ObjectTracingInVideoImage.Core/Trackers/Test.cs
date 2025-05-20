using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace ObjectTracingInVideoImage.Core.Trackers
{
    public class Test : IObjectTracker
    {
        private Mat? _template;
        private Size _templateSize;

        public void Initialize(Mat initialFrame, Rectangle selection)
        {
            _template?.Dispose();

            if (selection.X < 0 || selection.Y < 0 ||
                selection.Right > initialFrame.Width ||
                selection.Bottom > initialFrame.Height)
                throw new ArgumentException("Selection out of bounds");

            _template = new Mat(initialFrame, selection);
            _templateSize = selection.Size;
        }

        public Rectangle? Track(Mat frame)
        {
            if (_template == null || frame == null || frame.IsEmpty)
                return null;

            var result = new Mat();
            CvInvoke.MatchTemplate(frame, _template, result, TemplateMatchingType.CcorrNormed);

            // szukamy maksymalnej korelacji
            result.MinMax(out _, out double[] maxVal, out _, out Point[] maxLoc);

            if (maxVal[0] < 0.7) // próg – do eksperymentowania
                return null;

            return new Rectangle(maxLoc[0], _templateSize);
        }

        public void Dispose()
        {
            _template?.Dispose();
        }
    }
}
