using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using ObjectTracingInVideoImage.Core.Enums;

namespace ObjectTracingInVideoImage.Core.Testing.Logging
{
    public class TrackingLogger
    {
        private readonly List<TrackingLogEntry> _entries = new();
        private readonly Stopwatch _stopwatch = new();

        public void Log(int frame, bool detected, double? iou, TrackerType? trackerType)
        {
            if (frame == 0) _stopwatch.Restart();
            _entries.Add(new TrackingLogEntry
            {
                FrameNumber = frame,
                Detected = detected,
                IoU = iou,
                TrackerType = trackerType
            });
        }

        public void SaveToCsv(string imageDirectory, string trackerName)
        {
            _stopwatch.Stop();
            var fullPath = GetFullPathToSaveBenchmarkData(imageDirectory, trackerName);

            using var writer = new StreamWriter(fullPath);
            writer.WriteLine("Frame,Detected,IoU,TrackerType");

            foreach (var e in _entries)
            {
                string line = string.Format(CultureInfo.InvariantCulture,
                    "{0},{1},{2},{3}",
                    e.FrameNumber,
                    e.Detected,
                    e.IoU,
                    e.TrackerType);

                writer.WriteLine(line);
            }
            var averageFps = _entries.Count > 0 ? 1000.0 / (_stopwatch.ElapsedMilliseconds / _entries.Count) : 0;
            writer.WriteLine(averageFps.ToString());
            _entries.Clear();
        }

        private string GetFullPathToSaveBenchmarkData(string imageDirectory, string trackerName)
        {
            string? directory = Path.GetDirectoryName(imageDirectory);
            string folderName = Path.GetFileName(directory ?? string.Empty);
            string baseDir = AppContext.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\..\.."));
            var folderPath = Path.Combine(projectRoot, "TrackingResults");
            Directory.CreateDirectory(folderPath);
            string dateStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
            string resultFileName = $"{folderName}_{trackerName}_{dateStamp}.csv";
            return Path.Combine(folderPath, resultFileName);
        }
    }
}
