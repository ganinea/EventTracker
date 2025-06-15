namespace EventTracker.Domain.Model;

public class Parcel
{
    public long Id { get; set; }
    public DateTime? PickupDateTimeUtc { get; set; }
    public DateTime? DeliveryDateTimeUtc { get; set; }

    public ScanEvent? LastEvent { get; set; }
}