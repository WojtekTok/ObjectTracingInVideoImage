using System.Drawing;

namespace ObjectTracingInVideoImage.Core.KalmanFilter
{
    public class KalmanData
    {
        private const int MaxPoints = 40;
        public List<Point> PredictedPath { get; } = new();
        public List<Point> ObservedPath { get; } = new();

        public void AddPredictedPoint(PointF point)
        {
            if (PredictedPath.Count >= MaxPoints)
                PredictedPath.RemoveAt(0);
            PredictedPath.Add(Point.Round(point));
        }

        public void AddObservedPoint(PointF point)
        {
            if (ObservedPath.Count >= MaxPoints)
                ObservedPath.RemoveAt(0);
            ObservedPath.Add(Point.Round(point));
        }

        public void Clear()
        {
            PredictedPath.Clear();
            ObservedPath.Clear();
        }
    }
}
