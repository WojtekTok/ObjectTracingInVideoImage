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
            var lines = File.ReadAllLines(_csvPath).Skip(1);

            var frameList = new List<int>();
            var iouList = new List<double>();
            var detectedList = new List<bool>();
            var trackerList = new List<string?>();

            var colorMap = new Dictionary<string, Color>
            {
                ["kcf"] = Color.Orange,
                ["csrt"] = Color.DodgerBlue,
                ["mil"] = Color.MediumPurple,
                ["siammask"] = Color.DeepPink,
            };

            foreach (var line in File.ReadLines(_csvPath))
            {
                var parts = line.Split(',');

                if (parts.Length >= 4 &&
                    int.TryParse(parts[0], out int frame) &&
                    bool.TryParse(parts[1], out bool detected) &&
                    double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double iou))
                {
                    frameList.Add(frame);
                    detectedList.Add(detected);
                    iouList.Add(iou);
                    trackerList.Add(string.IsNullOrWhiteSpace(parts[3]) ? null : parts[3].Trim().ToLowerInvariant());
                }
            }

            benchmarkPlot.Plot.Clear();
            var segments = new List<(List<double> x, List<double> y, Color color)>();
            List<double> currentX = new();
            List<double> currentY = new();
            string? currentTracker = trackerList[0];

            for (int i = 0; i < frameList.Count; i++)
            {
                string? tracker = trackerList[i];

                if (tracker != currentTracker || i == frameList.Count - 1)
                {
                    if (currentX.Count > 0)
                    {
                        var color = currentTracker != null && colorMap.TryGetValue(currentTracker, out var c) ? c : Color.Orange;
                        segments.Add((new List<double>(currentX), new List<double>(currentY), color));
                    }

                    currentX.Clear();
                    currentY.Clear();
                    currentTracker = tracker;
                }

                currentX.Add(frameList[i]);
                currentY.Add(iouList[i]);
            }

            if (currentX.Count > 0)
            {
                var color = currentTracker != null && colorMap.TryGetValue(currentTracker, out var c) ? c : Color.Orange;
                segments.Add((currentX, currentY, color));
            }

            //foreach (var segment in segments)
            //{
            //    if (segment.x.Count == 1)
            //    {
            //        // Pojedynczy punkt – narysuj jako marker
            //        var scatter = benchmarkPlot.Plot.Add.ScatterPoints(
            //            xs: segment.x.ToArray(),
            //            ys: segment.y.ToArray(),
            //            color: ScottPlot.Color.FromColor(segment.color)
            //        );
            //        scatter.MarkerSize = 2; // Ustaw rozmiar markera
            //    }
            //    else
            //    {
            //        // Normalna linia
            //        benchmarkPlot.Plot.Add.ScatterLine(
            //            xs: segment.x.ToArray(),
            //            ys: segment.y.ToArray(),
            //            color: ScottPlot.Color.FromColor(segment.color)
            //        );
            //    }
            //}

            foreach (var segment in segments)
            {
                benchmarkPlot.Plot.Add.ScatterLine(segment.x.ToArray(), segment.y.ToArray(), color: ScottPlot.Color.FromColor(segment.color));
            }


            //scatter.MarkerSize = 0;
            benchmarkPlot.Plot.Axes.Bottom.Label.Text = "";
            benchmarkPlot.Plot.Title("IoU over Time");
            benchmarkPlot.Plot.Axes.Left.Label.Text = "IoU";
            benchmarkPlot.Plot.Legend.IsVisible = false;
            benchmarkPlot.Plot.SetStyle(new ScottPlot.PlotStyles.Dark());
            benchmarkPlot.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();

            //detectedPlot.Plot.Clear();
            //var detectedScatter = detectedPlot.Plot.Add.Scatter(
            //    detectionFrames.ToArray(),
            //    detectionValues.ToArray());

            //scatter.MarkerSize = 10;
            //scatter.MarkerColor = detectionColors.ToArray();
            //scatter.LineStyle = LineStyle.None;
            //scatter.

            //detectedPlot.Plot.Axes.Bottom.Label.Text = "Frame";
            //detectedPlot.Plot.Axes.Left.Label.Text = "";
            //detectedPlot.Plot.SetStyle(Style.Dark);
            //detectedPlot.Plot.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericFixed(0, 1); // tylko 1 poziom
            //detectedPlot.Plot.Axes.Left.MajorTickStyle.IsVisible = false;
            //detectedPlot.Plot.Axes.Left.Label.Text = "Detection";


            benchmarkPlot.Refresh();
        }
    }
}
