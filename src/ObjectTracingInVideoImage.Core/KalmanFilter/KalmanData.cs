using System.Drawing;

namespace ObjectTracingInVideoImage.Core.KalmanFilter
{
    public class KalmanData
    {
        public List<Point> PredictedPath { get; } = new();
        public List<Point> ObservedPath { get; } = new();

        public void AddPredictedPoint(Point point)
        {
            PredictedPath.Add(Point.Round(point));
        }

        public void AddObservedPoint(Point point)
        {
            ObservedPath.Add(Point.Round(point));
        }
    }
}
