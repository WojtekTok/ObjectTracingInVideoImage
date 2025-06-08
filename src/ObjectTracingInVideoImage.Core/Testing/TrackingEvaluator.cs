using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectTracingInVideoImage.Core.Testing
{
    public class TrackingEvaluator
    {
        private readonly GroundTruthData _groundTruth;
        private readonly List<double> _ious = new();
        private int _testedFrames = 0;

        public TrackingEvaluator(GroundTruthData groundTruth)
        {
            _groundTruth = groundTruth;
        }

        public void EvaluateFrame(int frameIndex, Rectangle? trackedBox)
        {
            var gt = _groundTruth.GetBox(frameIndex);
            if (gt == null || _groundTruth.IsOccluded(frameIndex) || _groundTruth.IsOutOfView(frameIndex))
                return;

            if (trackedBox.HasValue)
            {
                double iou = CalculateIoU(gt.Value, trackedBox.Value);
                _ious.Add(iou);
            }
            _testedFrames++;
        }

        public double MeanIoU => _ious.Count > 0 ? _ious.Average() : 0;
        public double MinIoU => _ious.Count > 0 ? _ious.Min() : 0;
        public double MaxIoU => _ious.Count > 0 ? _ious.Max() : 0;
        public int TestedFrames => _testedFrames;
        public int TotalFrames => _groundTruth.Count;

        public static double CalculateIoU(Rectangle a, Rectangle b)
        {
            var inter = Rectangle.Intersect(a, b);
            if (inter.IsEmpty) return 0;
            double interArea = inter.Width * inter.Height;
            double unionArea = a.Width * a.Height + b.Width * b.Height - interArea;
            return interArea / unionArea;
        }
    }

}
