using System.Net.Http.Json;
using System.Web;
using EventTracker.Common.Extensions;
using EventTracker.Domain;
using EventTracker.Domain.Model;
using EventTracker.Infrastructure.ScanEvents.Dto;
using Microsoft.Extensions.Logging;

namespace EventTracker.Infrastructure.ScanEvents;

internal sealed class ScanEventProvider(HttpClient httpClient,
     ILogger<ScanEventProvider> logger,
     ScanEventConfiguration config) : IScanEventProvider
{
     public async Task<List<ScanEvent>> Get(long? fromEventId, CancellationToken ct)
     {
          try
          {
               var uri = GetUrlWithParameters(fromEventId ?? 1, config.BatchSize);
               var dto = await httpClient.GetFromJsonAsync<ScanEventListDto>(uri, ct);
               return dto?.ScanEvents.Select(FromDtoIfPossible).NotNull().ToList() ?? [];
          }
          catch (Exception ex)
          {
               logger.LogError(ex, "Error getting scan events");
          }

          return [];
     }

     private string GetUrlWithParameters(long fromEventId, int limit)
     {
          var builder = new UriBuilder(config.Url);
          var query = HttpUtility.ParseQueryString(builder.Query);
          query["FromEventId"] = fromEventId.ToString();
          query["Limit"] = limit.ToString();
          builder.Query = query.ToString();
          return builder.ToString();
     }

     private ScanEvent? FromDtoIfPossible(ScanEventDto dto)
     {
          if (dto.EventId is null)
          {
               logger.LogWarning("EventId is null");
               return null;
          }

          if (dto.ParcelId is null)
          {
               logger.LogWarning("ScanEvent {EventId}: ParcelId is null", dto.EventId);
               return null;
          }

          if (dto.Type.IsNullOrWhiteSpace())
          {
               logger.LogWarning("ScanEvent {EventId}: Type is null", dto.EventId);
               return null;
          }

          if (dto.CreatedDateTimeUtc is null)
          {
               logger.LogWarning("ScanEvent {EventId}: CreatedDateTimeUtc is null", dto.EventId);
               return null;
          }

          return new ScanEvent
          {
               Id = dto.EventId.Value,
               ParcelId = dto.ParcelId.Value,
               Type = dto.Type!,
               StatusCode = dto.StatusCode,
               CreatedDateTimeUtc = dto.CreatedDateTimeUtc.Value,
               RunId = dto.User?.RunId
          };
     }
}