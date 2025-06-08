using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectTracingInVideoImage.Core.Testing
{
    public class GroundTruthData
    {
        private readonly List<Rectangle> _boxes;
        private readonly List<bool> _occlusion;
        private readonly List<bool> _outOfView;

        public GroundTruthData(string bboxPath, string occlusionPath, string outOfViewPath)
        {
            _boxes = File.ReadAllLines(bboxPath)
                .Select(line =>
                {
                    var parts = line.Split(',').Select(int.Parse).ToArray();
                    return new Rectangle(parts[0], parts[1], parts[2], parts[3]);
                }).ToList();

            _occlusion = File.ReadAllLines(occlusionPath)
                .Select(l => l.Trim() == "1").ToList();

            _outOfView = File.ReadAllLines(outOfViewPath)
                .Select(l => l.Trim() == "1").ToList();
        }

        public Rectangle? GetBox(int frame) =>
             frame >= 0 && frame < _boxes.Count ? _boxes[frame] : (Rectangle?)null;
        public bool IsOccluded(int frame) =>
            frame >= 0 && frame < _occlusion.Count && _occlusion[frame];
        public bool IsOutOfView(int frame) =>
            frame >= 0 && frame < _outOfView.Count && _outOfView[frame];
        public int Count => _boxes.Count;
    }

}
