using EventTracker.Domain.Data;
using EventTracker.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace EventTracker.Infrastructure.Storage.Services;

public class WorkerStateDataService(EventTrackerContext context)
    : IWorkerStateDataService
{
    public async Task<WorkerState?> Get(CancellationToken ct)
    {
        return await context.WorkerStates.FirstOrDefaultAsync(ct);
    }

    public async Task Save(WorkerState state)
    {
        if (context.Entry(state).State == EntityState.Detached)
        {
            context.WorkerStates.Add(state);
        }
        else
        {
            context.WorkerStates.Update(state);
        }
        await context.SaveChangesAsync();
    }
}