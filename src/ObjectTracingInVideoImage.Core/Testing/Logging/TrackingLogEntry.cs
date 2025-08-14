using ObjectTracingInVideoImage.Core.Enums;

namespace ObjectTracingInVideoImage.Core.Testing.Logging
{
    public class TrackingLogEntry
    {
        public int FrameNumber { get; set; }
        public bool Detected { get; set; }
        public double? IoU { get; set; }
        public TrackerType? TrackerType { get; set; }
    }
}
