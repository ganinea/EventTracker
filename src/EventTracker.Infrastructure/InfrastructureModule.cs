using EventTracker.Common;
using EventTracker.Infrastructure.Common;
using EventTracker.Infrastructure.ScanEvents;
using EventTracker.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventTracker.Infrastructure;

public class InfrastructureModule(IConfiguration configuration) : IModule
{
    public void AddServices(IServiceCollection services)
    {
        services.AddModules(new ScanEventModule(configuration),
            new StorageModule(configuration));
        
        services.AddCustomConfiguration<WorkerConfiguration>(configuration);
    }
}