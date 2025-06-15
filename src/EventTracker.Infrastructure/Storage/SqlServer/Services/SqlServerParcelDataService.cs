using System.Data;
using EventTracker.Domain.Data;
using EventTracker.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EventTracker.Infrastructure.Storage.SqlServer.Services;

public class SqlServerParcelDataService(EventTrackerContext context) : IParcelDataService
{
    public async Task Update(IEnumerable<Parcel> parcels)
    {
        var connection = (SqlConnection)context.Database.GetDbConnection();
        var wasClosed = connection.State == ConnectionState.Closed;
        if (wasClosed)
        {
            await connection.OpenAsync();
        }

        var parcelsTable = CreateParcelsTable(parcels);
        var parcelsParameter = CreateParcelsParameter(parcelsTable);
        await using var command = CreateUpdateParcelsCommand(connection, parcelsParameter);
        await command.ExecuteNonQueryAsync();

        if (wasClosed)
        {
            await connection.CloseAsync();
        }
    }

    private static DataTable CreateParcelsTable(IEnumerable<Parcel> parcels)
    {
        var dataTable = new DataTable();
        dataTable.Columns.Add("ParcelId", typeof(long));
        dataTable.Columns.Add("LastEventId", typeof(long));
        dataTable.Columns.Add("LastEventType", typeof(string));
        dataTable.Columns.Add("LastEventCreatedDateTimeUtc", typeof(DateTime));
        dataTable.Columns.Add("LastEventStatusCode", typeof(string));
        dataTable.Columns.Add("LastEventRunId", typeof(string));
        dataTable.Columns.Add("PickupDateTimeUtc", typeof(DateTime));
        dataTable.Columns.Add("DeliveryDateTimeUtc", typeof(DateTime));

        foreach (var parcel in parcels)
        {
            dataTable.Rows.Add(parcel.Id,
                parcel.LastEvent!.Id,
                parcel.LastEvent.Type,
                parcel.LastEvent.CreatedDateTimeUtc,
                parcel.LastEvent.StatusCode,
                parcel.LastEvent.RunId,
                parcel.PickupDateTimeUtc,
                parcel.DeliveryDateTimeUtc);
        }

        return dataTable;
    }

    private static SqlParameter CreateParcelsParameter(DataTable dataTable)
    {
        return new SqlParameter
        {
            ParameterName = "@Parcels",
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.ParcelTableType",
            Value = dataTable
        };
    }
    
    private static SqlCommand CreateUpdateParcelsCommand(SqlConnection connection, SqlParameter parcels)
    {
        var command = new SqlCommand("dbo.UpdateParcels", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add(parcels);

        return command;
    }
}