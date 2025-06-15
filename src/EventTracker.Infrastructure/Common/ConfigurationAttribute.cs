namespace EventTracker.Infrastructure.Common;

[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationAttribute(string root) : Attribute
{
    public string Root { get; set; } = root;
}
