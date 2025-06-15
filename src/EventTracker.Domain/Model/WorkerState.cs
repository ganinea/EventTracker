namespace EventTracker.Domain.Model;

public class WorkerState
{
    public long Id { get; init; } = 1;
    public long LastProcessedEventId { get; set; }
}