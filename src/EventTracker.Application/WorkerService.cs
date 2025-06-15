using EventTracker.Common.Extensions;
using EventTracker.Domain;
using EventTracker.Domain.Data;
using EventTracker.Domain.Model;
using Microsoft.Extensions.Logging;

namespace EventTracker.Application;

public class WorkerService(IScanEventProvider eventProvider,
    IWorkerStateDataService workerStateDataService,
    ILogger<WorkerService> logger,
    IParcelDataService parcelDataService)
{
    public async Task TrackEvents(CancellationToken ct)
    {
        logger.LogInformation("Worker service started");

        logger.LogInformation("Retrieving state");
        var workerState = await workerStateDataService.Get(ct);
        if (workerState == null)
        {
            workerState = new WorkerState();
            logger.LogInformation("No state found, default state created");
        }

        logger.LogInformation("Getting events");
        var events = await eventProvider.Get(workerState.LastProcessedEventId, ct);
        if (events.IsEmpty())
        {
            logger.LogInformation("No events loaded");
            return;
        }

        logger.LogInformation("Loaded {EventsCount} events", events.Count);

        logger.LogInformation("Updating parcels");
        await parcelDataService.Update(ToParcels(events));

        logger.LogInformation("Saving worker state");
        workerState.LastProcessedEventId = events.Max(x => x.Id);
        await workerStateDataService.Save(workerState);

        logger.LogInformation("Worker service finished");
    }

    private IReadOnlyCollection<Parcel> ToParcels(IEnumerable<ScanEvent> events)
    {
        var parcelById = new Dictionary<long, Parcel>();
        foreach (var scanEvent in events)
        {
            var parcel = parcelById.GetValueOrDefault(scanEvent.ParcelId);
            if (parcel == null)
            {
                parcel = new Parcel{Id = scanEvent.ParcelId};
                parcelById.Add(scanEvent.ParcelId, parcel);
            }

            if (parcel.LastEvent == null || parcel.LastEvent.Id < scanEvent.Id)
            {
                parcel.LastEvent = scanEvent;
                parcel.PickupDateTimeUtc = scanEvent.Type == ScanEventType.PickUp
                    ? scanEvent.CreatedDateTimeUtc
                    : parcel.PickupDateTimeUtc;
                parcel.DeliveryDateTimeUtc = scanEvent.Type == ScanEventType.Delivery
                    ? scanEvent.CreatedDateTimeUtc
                    : parcel.DeliveryDateTimeUtc;
            }
        }

        return parcelById.Values;
    }
}