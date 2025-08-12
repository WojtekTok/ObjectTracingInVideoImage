using System.Globalization;
using ScottPlot;
using ScottPlot.Plottables;
using Color = System.Drawing.Color;

namespace ObjectTracingInVideoImage.App.Forms
{
    public partial class BenchmarkForm : Form
    {
        private readonly string _csvPath;

        public BenchmarkForm(string csvPath)
        {
            InitializeComponent();
            _csvPath = csvPath;
            LoadChart();
        }

        private void LoadChart()
        {
            var (frameList, iouList, detectedList, trackerList) = LoadCsvData(_csvPath);

            benchmarkPlot.Plot.Clear();

            var segments = BuildSegments(frameList, iouList, detectedList, trackerList);

            PlotSegments(segments);

            StylePlot();
            benchmarkPlot.Refresh();
        }

        private (List<int> frames, List<double> ious, List<bool> detected, List<string?> trackers) LoadCsvData(string path)
        {
            var frames = new List<int>();
            var ious = new List<double>();
            var detected = new List<bool>();
            var trackers = new List<string?>();

            foreach (var line in File.ReadLines(path).Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length >= 4 &&
                    int.TryParse(parts[0], out int frame) &&
                    bool.TryParse(parts[1], out bool det) &&
                    double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double iou))
                {
                    frames.Add(frame);
                    detected.Add(det);
                    trackers.Add(string.IsNullOrWhiteSpace(parts[3]) ? null : parts[3].Trim().ToLowerInvariant());
                    ious.Add(iou);
                }
            }

            return (frames, ious, detected, trackers);
        }

        private List<(List<double> x, List<double> y, Color color, string label)> BuildSegments(
            List<int> frameList,
            List<double> iouList,
            List<bool> detectedList,
            List<string?> trackerList)
        {
            var colorMap = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
            {
                ["kcf"] = Color.Orange,
                ["csrt"] = Color.DodgerBlue,
                ["mil"] = Color.MediumPurple,
                ["siammask"] = Color.DeepPink,
            };

            var labelMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["kcf"] = "KCF",
                ["csrt"] = "CSRT",
                ["mil"] = "MIL",
                ["siammask"] = "SiamMask",
            };

            Color noDetectionColor = Color.LightGray;
            string noDetectionLabel = "Failed";

            var segments = new List<(List<double> x, List<double> y, Color color, string label)>();

            List<double> currentX = new();
            List<double> currentY = new();
            string? currentTracker = trackerList[0];
            bool currentDetected = detectedList[0];

            for (int i = 0; i < frameList.Count; i++)
            {
                string? tracker = trackerList[i];
                bool detected = detectedList[i];

                if (tracker != currentTracker || detected != currentDetected)
                {
                    if (currentX.Count > 0)
                    {
                        var color = currentDetected
                            ? (currentTracker != null && colorMap.TryGetValue(currentTracker, out var c) ? c : Color.Orange)
                            : noDetectionColor;

                        var label = currentDetected
                            ? (currentTracker != null && labelMap.TryGetValue(currentTracker, out var l) ? l : currentTracker)
                            : noDetectionLabel;

                        segments.Add((new List<double>(currentX), new List<double>(currentY), color, label));

                        currentX.Clear();
                        currentY.Clear();
                        currentX.Add(frameList[i - 1]);
                        currentY.Add(iouList[i - 1]);
                    }

                    currentTracker = tracker;
                    currentDetected = detected;
                }

                currentX.Add(frameList[i]);
                currentY.Add(iouList[i]);
            }

            if (currentX.Count > 0)
            {
                var color = currentDetected
                    ? (currentTracker != null && colorMap.TryGetValue(currentTracker, out var c) ? c : Color.Orange)
                    : noDetectionColor;

                var label = currentDetected
                    ? (currentTracker != null && labelMap.TryGetValue(currentTracker, out var l) ? l : currentTracker)
                    : noDetectionLabel;

                segments.Add((currentX, currentY, color, label));
            }

            return segments;
        }

        private void PlotSegments(List<(List<double> x, List<double> y, Color color, string label)> segments)
        {
            var legendAdded = new HashSet<string>();

            foreach (var segment in segments)
            {
                var line = benchmarkPlot.Plot.Add.ScatterLine(
                    segment.x.ToArray(),
                    segment.y.ToArray(),
                    color: ScottPlot.Color.FromColor(segment.color)
                );

                if (!legendAdded.Contains(segment.label))
                {
                    line.LegendText = segment.label;
                    legendAdded.Add(segment.label);
                }
            }
        }

        private void StylePlot()
        {
            string fileName = Path.GetFileName(_csvPath);
            benchmarkPlot.Plot.Title($"IoU over Time – {fileName}");
            benchmarkPlot.Plot.Axes.Left.Label.Text = "IoU";
            benchmarkPlot.Plot.SetStyle(new ScottPlot.PlotStyles.Dark());
            benchmarkPlot.Plot.Legend.IsVisible = true;
            benchmarkPlot.Plot.Legend.Alignment = ScottPlot.Alignment.LowerLeft;
            benchmarkPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();
        }

    }
}
