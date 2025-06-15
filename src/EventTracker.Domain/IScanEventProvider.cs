using EventTracker.Domain.Model;

namespace EventTracker.Domain;

public interface IScanEventProvider
{
     Task<List<ScanEvent>> Get(long? fromEventId, CancellationToken ct);
}