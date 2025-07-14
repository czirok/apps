using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Yaml.Localization;

namespace YamlLocalizationTests;

public class CoreFixture : IDisposable
{
    public ServiceProvider Services { get; }

    public CoreFixture()
    {
        var collection = new ServiceCollection();
        collection.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
        collection.AddYamlEmbeddedResourceLocalization(typeof(CoreFixture).Assembly);
        Services = collection.BuildServiceProvider();
    }

    public void Dispose()
    {
        Services?.Dispose();
    }
}
