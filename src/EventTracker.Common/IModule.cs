using Microsoft.Extensions.DependencyInjection;

namespace EventTracker.Common;

public interface IModule
{
    void AddServices(IServiceCollection services);
}