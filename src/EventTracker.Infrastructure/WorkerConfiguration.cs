using EventTracker.Common;

namespace EventTracker.Infrastructure;

public class WorkerConfiguration : ICustomConfiguration
{
    public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(5);
}