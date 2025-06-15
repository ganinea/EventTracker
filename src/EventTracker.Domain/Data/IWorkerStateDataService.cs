using EventTracker.Domain.Model;

namespace EventTracker.Domain.Data;

public interface IWorkerStateDataService
{
    Task<WorkerState?> Get(CancellationToken ct);
    Task Save(WorkerState state);
}