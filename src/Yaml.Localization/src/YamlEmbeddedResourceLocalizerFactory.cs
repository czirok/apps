using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;
using YamlDotNet.Serialization;

namespace Yaml.Localization;

/// <summary>
/// Yaml embedded resource localizer factory.
/// </summary>
/// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
public sealed class YamlEmbeddedResourceLocalizerFactory(ILoggerFactory loggerFactory) : IStringLocalizerFactory
{
	readonly ConcurrentDictionary<string, YamlEmbeddedResourceLocalizer> _localizerCache = new();
	readonly ConcurrentDictionary<Assembly, HashSet<string>> _resourceCache = new();
	readonly IDeserializer _deserializer = new StaticDeserializerBuilder(new StaticAoTContext()).Build();

	/// <inheritdoc />
	public IStringLocalizer Create(Type resourceSource)
	{
		ArgumentNullException.ThrowIfNull(resourceSource);

		if (_localizerCache.TryGetValue(resourceSource.AssemblyQualifiedName!, out var localizer)) return localizer;


		var resourceFiles = _resourceCache.GetOrAdd(resourceSource.Assembly,
		assembly => assembly.GetManifestResourceNames()
			.Where(name => name.EndsWith(Extensions.Yaml))
			.ToHashSet());

		localizer = new YamlEmbeddedResourceLocalizer(
			resourceSource.Assembly,
			resourceSource.FullName!,
			_deserializer,
			resourceFiles,
			loggerFactory.CreateLogger<YamlEmbeddedResourceLocalizer>());

		_localizerCache[resourceSource.AssemblyQualifiedName!] = localizer;

		return localizer;
	}

	/// <inheritdoc />
	public IStringLocalizer Create(string baseName, string location)
	{
		throw new NotSupportedException();
	}
}