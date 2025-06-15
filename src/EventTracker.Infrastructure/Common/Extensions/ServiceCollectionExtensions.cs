using System.Reflection;
using EventTracker.Common;
using EventTracker.Common.Extensions;
using EventTracker.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EventTracker.Infrastructure.Common;

public static class ServiceCollectionExtensions
{
    public static void AddModules(this IServiceCollection self, params IModule[] modules)
    {
        modules.ForEach(x => x.AddServices(self));
    }

    public static void AddCustomConfiguration<TConfig>(this IServiceCollection self, IConfiguration configuration)
        where TConfig : class, ICustomConfiguration, new()
    {
        var key = GetRoot(typeof(TConfig));

        self.AddOptions<TConfig>().Bind(configuration.GetSection(key));
        self.AddSingleton(x => x.GetRequiredService<IOptions<TConfig>>().Value);
    }

    private static string GetRoot(Type configType)
    {
        var attribute = configType.GetCustomAttribute(typeof(ConfigurationAttribute), false);
        var key = attribute is ConfigurationAttribute configurationAttribute
            ? configurationAttribute.Root
            : configType.Name.Replace("Configuration", "");
        return key;
    }
}
