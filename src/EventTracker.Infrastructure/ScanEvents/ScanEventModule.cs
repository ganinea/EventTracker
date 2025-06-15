using EventTracker.Common;
using EventTracker.Domain;
using EventTracker.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace EventTracker.Infrastructure.ScanEvents;

public class ScanEventModule(IConfiguration configuration) : IModule
{
    public void AddServices(IServiceCollection services)
    {
        services.AddCustomConfiguration<ScanEventConfiguration>(configuration);

        services.AddScoped<IScanEventProvider, ScanEventProvider>();
        services.AddHttpClient<IScanEventProvider, ScanEventProvider>()
            .AddPolicyHandler((provider, _) =>
            {
                var config = provider.GetRequiredService<ScanEventConfiguration>();
                return GetRetryPolicy(config);
            });
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ScanEventConfiguration config)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(config.RetryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}