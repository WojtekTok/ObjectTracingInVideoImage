using Emgu.CV;
using Emgu.CV.Structure;
using ObjectTracingInVideoImage.Core.KalmanFilter;

namespace ObjectTracingInVideoImage.App.Visuralizers
{
    public static class KalmanVisualizer
    {
        public static void DrawPaths(Mat frame, KalmanData data)
        {
            var img = frame.ToImage<Bgr, byte>();
            var predictedPath = data.PredictedPath;
            var observedPath = data.ObservedPath;

            for (int i = 1; i < predictedPath.Count; i++)
                CvInvoke.Line(img, predictedPath[i - 1], predictedPath[i], new MCvScalar(255, 0, 0), 2); // niebieska: predykcja

            for (int i = 1; i < observedPath.Count; i++)
                CvInvoke.Line(img, observedPath[i - 1], observedPath[i], new MCvScalar(0, 0, 255), 2); // czerwona: obserwacja

            if (predictedPath.Count > 0)
                CvInvoke.Circle(img, predictedPath[^1], 4, new MCvScalar(255, 0, 0), -1);

            if (observedPath.Count > 0)
                CvInvoke.Circle(img, observedPath[^1], 4, new MCvScalar(0, 0, 255), -1);

            img.Mat.CopyTo(frame);
        }
    }
}
