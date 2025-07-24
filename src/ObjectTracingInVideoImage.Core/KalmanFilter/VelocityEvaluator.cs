using System.Drawing;

namespace ObjectTracingInVideoImage.Core.KalmanFilter
{
    public static class VelocityEvaluator
    {
        public static float? ComputeVelocity(List<Point> points)
        {
            int count = points.Count;

            if (count < 10)
                return null;

            int windowSize = Math.Min(10, count - 1);
            float totalDistance = 0;

            for (int i = count - windowSize; i < count - 1; i++)
            {
                Point p1 = points[i];
                Point p2 = points[i + 1];

                float dx = p2.X - p1.X;
                float dy = p2.Y - p1.Y;
                totalDistance += MathF.Sqrt(dx * dx + dy * dy);
            }

            return totalDistance / windowSize;
        }
    }
}
