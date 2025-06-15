using EventTracker.Common;
using EventTracker.Infrastructure.Common;

namespace EventTracker.Infrastructure.ScanEvents;

public class ScanEventConfiguration : ICustomConfiguration
{
    public string Url { get; set; } = "http://localhost/v1/scans/scanevents";
    public int BatchSize { get; set; } = 100;
    public int RetryCount { get; set; } = 3;
}