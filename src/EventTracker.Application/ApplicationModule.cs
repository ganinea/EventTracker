using EventTracker.Common;
using Microsoft.Extensions.DependencyInjection;

namespace EventTracker.Application;

public class ApplicationModule : IModule
{
    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<WorkerService>();
    }
}