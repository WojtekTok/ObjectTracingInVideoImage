using ObjectTracingInVideoImage.Core.Enums;
using ObjectTracingInVideoImage.Core.Trackers;

namespace ObjectTracingInVideoImage.Core.Factories
{
    public static class TrackerFactory
    {
        public static IObjectTracker Create(TrackerType type)
        {
            return type switch
            {
                TrackerType.KCF => new KcfObjectTracker(),
                TrackerType.Test => new Test(),
                TrackerType.CSRT => new CsrtObjectTracker(),
                TrackerType.Siammask => new SiammaskObjectTracker(),
                TrackerType.MIL => new MilObjectTracker(),
                TrackerType.Hybrid_KCF => new HybridObjectTracker(Create(TrackerType.KCF)),
                TrackerType.Hybrid_CSRT => new HybridObjectTracker(Create(TrackerType.CSRT)),
                TrackerType.Hybrid_MIL => new HybridObjectTracker(Create(TrackerType.MIL)),
                TrackerType.Hybrid => new HybridObjectTrackerv2(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
