using System.Drawing;
using Emgu.CV;

namespace ObjectTracingInVideoImage.Core.KalmanFilter
{
    public class KalmanFilter2D : IKalmanFilter2D
    {
        private readonly Emgu.CV.KalmanFilter _kalman;
        private readonly Matrix<float> _state;
        private readonly Matrix<float> _measurement;
        public KalmanData _kalmanData = new();

        public KalmanFilter2D()
        {
            _kalman = new Emgu.CV.KalmanFilter(4, 2, 0);
            _state = new Matrix<float>(4, 1);
            _measurement = new Matrix<float>(2, 1);

            var transitionMatrix = new Matrix<float>(new float[,]
            {
                {1, 0, 1, 0},
                {0, 1, 0, 1},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
            });
            transitionMatrix.Mat.CopyTo(_kalman.TransitionMatrix);

            var measurementMatrix = new Matrix<float>(new float[,]
            {
                {1, 0, 0, 0},
                {0, 1, 0, 0}
            });
            measurementMatrix.Mat.CopyTo(_kalman.MeasurementMatrix);

            var processNoise = new Matrix<float>(new float[,]
            {
                {1e-2f, 0, 0, 0},
                {0, 1e-2f, 0, 0},
                {0, 0, 1e-2f, 0},
                {0, 0, 0, 1e-2f}
            });
            processNoise.Mat.CopyTo(_kalman.ProcessNoiseCov);

            var measurementNoise = new Matrix<float>(new float[,] 
            { 
                { 1e-1f, 0 }, 
                { 0, 1e-1f } 
            });
            measurementNoise.Mat.CopyTo(_kalman.MeasurementNoiseCov);

            var errorCov = new Matrix<float>(new float[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            });
            errorCov.Mat.CopyTo(_kalman.ErrorCovPost);
        }

        public void Init(PointF position)
        {
            _state[0, 0] = position.X;
            _state[1, 0] = position.Y;
            _state[2, 0] = 0;
            _state[3, 0] = 0;
            _state.Mat.CopyTo(_kalman.StatePost);
        }

        public PointF Predict()
        {
            var prediction = _kalman.Predict();
            var data = prediction.GetData() as float[,];
            return new PointF(data[0, 0], data[1, 0]);
        }

        public void Correct(PointF observedPosition)
        {
            _measurement[0, 0] = observedPosition.X;
            _measurement[1, 0] = observedPosition.Y;
            _kalman.Correct(_measurement.Mat);
        }
    }
}
