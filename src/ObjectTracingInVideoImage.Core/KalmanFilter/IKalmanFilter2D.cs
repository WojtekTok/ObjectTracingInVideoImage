using System.Drawing;

namespace ObjectTracingInVideoImage.Core.KalmanFilter
{
    public interface IKalmanFilter2D
    {
        public void Init(PointF position);

        public PointF Predict();

        public void Correct(PointF observedPosition);
    }
}
