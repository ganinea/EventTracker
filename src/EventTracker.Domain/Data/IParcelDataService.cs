using EventTracker.Domain.Model;

namespace EventTracker.Domain.Data;

public interface IParcelDataService
{
    Task Update(IEnumerable<Parcel> parcels);
}