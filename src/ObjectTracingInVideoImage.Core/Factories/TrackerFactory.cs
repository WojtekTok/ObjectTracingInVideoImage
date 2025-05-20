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
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
