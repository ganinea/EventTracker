CREATE TYPE ParcelTableType AS TABLE
    (
    ParcelId BIGINT,
    LastEventId BIGINT,
    LastEventType NVARCHAR(50),
    LastEventCreatedDateTimeUtc DATETIME2,
    LastEventStatusCode NVARCHAR(50),
    LastEventRunId NVARCHAR(50),
    PickupDateTimeUtc DATETIME2,
    DeliveryDateTimeUtc DATETIME2
    );
GO

CREATE PROCEDURE UpdateParcels
    @Parcels ParcelTableType READONLY
AS
BEGIN
    SET NOCOUNT ON;

MERGE Parcels AS target
    USING @Parcels AS source
    ON target.Id = source.ParcelId
    WHEN MATCHED AND target.LastEvent_Id < source.LastEventId THEN
UPDATE SET
    LastEvent_Id = source.LastEventId,
    LastEvent_Type = source.LastEventType,
    LastEvent_CreatedDateTimeUtc = source.LastEventCreatedDateTimeUtc,
    LastEvent_StatusCode = source.LastEventStatusCode,
    LastEvent_RunId = source.LastEventRunId,
    PickupDateTimeUtc = CASE
    WHEN source.PickupDateTimeUtc is not null THEN source.PickupDateTimeUtc
    ELSE target.PickupDateTimeUtc
END,
            DeliveryDateTimeUtc = CASE 
                WHEN source.DeliveryDateTimeUtc is not null THEN source.DeliveryDateTimeUtc 
                ELSE target.DeliveryDateTimeUtc
END
WHEN NOT MATCHED THEN
        INSERT (Id, LastEvent_Id, LastEvent_Type, LastEvent_CreatedDateTimeUtc, LastEvent_StatusCode, LastEvent_RunId,
                PickupDateTimeUtc, DeliveryDateTimeUtc)
        VALUES (source.ParcelId, source.LastEventId, source.LastEventType, source.LastEventCreatedDateTimeUtc, source.LastEventStatusCode, source.LastEventRunId,
                CASE WHEN source.PickupDateTimeUtc is not null THEN source.PickupDateTimeUtc ELSE NULL END,
                CASE WHEN source.DeliveryDateTimeUtc is not null THEN source.DeliveryDateTimeUtc ELSE NULL END);
END