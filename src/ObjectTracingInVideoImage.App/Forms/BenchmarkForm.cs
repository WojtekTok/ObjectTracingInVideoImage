using System.Data;
using System.Globalization;
using Color = System.Drawing.Color;

namespace ObjectTracingInVideoImage.App.Forms
{
    public partial class BenchmarkForm : Form
    {
        private readonly string _csvPath;
        private List<int> _frames;
        private List<double> _ious;
        private List<bool> _detected;
        private List<string?> _trackers;

        public BenchmarkForm(string csvPath)
        {
            InitializeComponent();
            _csvPath = csvPath;
            LoadChart();
            ShowSummary();
        }

        private void LoadChart()
        {
            LoadCsvData(_csvPath);
            if (_ious.Count() < 1)
                throw new ArgumentOutOfRangeException("Could not retreive IoU list");

            benchmarkPlot.Plot.Clear();

            var segments = BuildSegments();

            PlotSegments(segments);

            benchmarkPlot.Refresh();
        }

        private void LoadCsvData(string path)
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

            _frames = frames;
            _ious = ious;
            _detected = detected;
            _trackers = trackers;
        }

        private List<(List<double> x, List<double> y, Color color, string label)> BuildSegments()
        {
            var colorMap = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
            {
                ["kcf"] = Color.Orange,
                ["csrt"] = Color.MediumTurquoise,
                ["mil"] = Color.DodgerBlue,
                ["siammask"] = Color.MediumOrchid,
            };

            var labelMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["kcf"] = "KCF",
                ["csrt"] = "CSRT",
                ["mil"] = "MIL",
                ["siammask"] = "SiamMask",
            };

            Color noDetectionColor = Color.DarkGray;
            string noDetectionLabel = "Failed";

            var segments = new List<(List<double> x, List<double> y, Color color, string label)>();

            List<double> currentX = new();
            List<double> currentY = new();
            string? currentTracker = _trackers[0];
            bool currentDetected = _detected[0];

            for (int i = 0; i < _frames.Count; i++)
            {
                string? tracker = _trackers[i];
                bool detected = _detected[i];

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
                        currentX.Add(_frames[i - 1]);
                        currentY.Add(_ious[i - 1]);
                    }

                    currentTracker = tracker;
                    currentDetected = detected;
                }

                currentX.Add(_frames[i]);
                currentY.Add(_ious[i]);
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

        private void ShowSummary()
        {
            double? totalTime = GetTotalTimeFromCsv(_csvPath);
            var trackerGroups = CalculateTrackerStats(_frames, _trackers, _ious, _detected);
            DataTable table = BuildSummaryTable(trackerGroups, totalTime, _ious, _detected, _frames.Count);
            DisplayResults(table);
        }

        private double? GetTotalTimeFromCsv(string csvPath)
        {
            var lastLine = File.ReadLines(csvPath).Last();
            if (double.TryParse(lastLine.Split(',')[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double sec))
                return sec;
            return null;
        }

        private Dictionary<string, (double AvgIoU, double AccuracyPercent, int UsageCount)> CalculateTrackerStats(
            List<int> frames, List<string> trackers, List<double> ious, List<bool> detected)
        {
            return frames
                .Select((f, i) => new
                {
                    Tracker = trackers[i] ?? "unknown",
                    IoU = ious[i],
                    Detected = detected[i]
                })
                .GroupBy(x => x.Tracker)
                .ToDictionary(
                    g => g.Key,
                    g => (
                        AvgIoU: g.Average(r => r.IoU),
                        AccuracyPercent: g.Count(r => r.Detected) * 100.0 / g.Count(),
                        UsageCount: g.Count()
                    )
                );
        }

        private DataTable BuildSummaryTable(
            Dictionary<string, (double AvgIoU, double AccuracyPercent, int UsageCount)> trackerGroups,
            double? totalTime,
            List<double> ious,
            List<bool> detected,
            int totalCount)
        {
            var distinctTrackers = trackerGroups.Keys.ToList();
            bool multipleTrackers = distinctTrackers.Count > 1;

            DataTable table = new DataTable();
            table.Columns.Add("Tracker");
            table.Columns.Add("Mean FPS");
            table.Columns.Add("Mean IoU");
            table.Columns.Add("% of Positive Results");
            if (multipleTrackers)
                table.Columns.Add("% of Times Used");

            if (multipleTrackers)
            {
                table.Rows.Add(
                    "General",
                    totalTime?.ToString("F2") ?? "-",
                    ious.Average().ToString("F3"),
                    (detected.Count(d => d) * 100.0 / detected.Count).ToString("F2"),
                    "-"
                );

                foreach (var kv in trackerGroups)
                {
                    var tracker = kv.Key;
                    var stats = kv.Value;
                    table.Rows.Add(
                        tracker,
                        "-",
                        stats.AvgIoU.ToString("F3"),
                        stats.AccuracyPercent.ToString("F2"),
                        ((double)stats.UsageCount / totalCount * 100.0).ToString("F2")
                    );
                }
            }
            else
            {
                var tracker = distinctTrackers.First();
                var stats = trackerGroups[tracker];
                table.Rows.Add(
                    tracker,
                    totalTime?.ToString("F2") ?? "-",
                    stats.AvgIoU.ToString("F3"),
                    stats.AccuracyPercent.ToString("F2")
                );
            }

            return table;
        }

        private void DisplayResults(DataTable table)
        {
            if (resultsGrid == null)
            {
                resultsGrid = new DataGridView
                {
                    Dock = DockStyle.Bottom,
                    Height = 120,
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };
                Controls.Add(resultsGrid);
            }

            resultsGrid.DataSource = table;
        }

    }
}
