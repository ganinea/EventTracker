namespace EventTracker.Domain.Model;

public class ScanEvent
{
    public long Id { get; set; }

    public long ParcelId { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedDateTimeUtc { get; set; }
    public string? StatusCode { get; set; } = string.Empty;
    public string? RunId { get; set; } = string.Empty;
}