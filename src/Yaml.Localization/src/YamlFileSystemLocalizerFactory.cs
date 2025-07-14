using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;
using YamlDotNet.Serialization;

namespace Yaml.Localization;

/// <summary>
/// Yaml file system localizer factory.
/// </summary>
public sealed class YamlFileSystemLocalizerFactory : IStringLocalizerFactory
{
	readonly ConcurrentDictionary<string, YamlFileSystemLocalizer> _localizerCache = new();
	readonly IDeserializer _deserializer = new StaticDeserializerBuilder(new StaticAoTContext()).Build();
	readonly ILogger<YamlFileSystemLocalizer> _logger;
	readonly string _i18nPath;
	private readonly string[] _resourceFiles;

	/// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
	/// <param name="configuration">The <see cref="IConfiguration"/>.</param>
	/// <param name="resourceFiles">The list of resource files to search for.</param>
	public YamlFileSystemLocalizerFactory(
		ILoggerFactory loggerFactory,
		IConfiguration configuration,
		string[] resourceFiles)
	{
		ArgumentNullException.ThrowIfNull(loggerFactory);
		ArgumentNullException.ThrowIfNull(configuration);
		ArgumentNullException.ThrowIfNull(resourceFiles);

		_logger = loggerFactory.CreateLogger<YamlFileSystemLocalizer>();
		_i18nPath = Path.Combine(AppContext.BaseDirectory, configuration["i18nPath"]!);
		_resourceFiles = resourceFiles;
	}

	/// <inheritdoc />
	public IStringLocalizer Create(Type resourceSource)
	{
		ArgumentNullException.ThrowIfNull(resourceSource);
		ArgumentException.ThrowIfNullOrWhiteSpace(resourceSource.AssemblyQualifiedName, "Type.AssemblyQualifiedName cannot be null or empty.");

		if (_localizerCache.TryGetValue(resourceSource.AssemblyQualifiedName, out var localizer)) return localizer;

		var typeInfo = resourceSource.GetTypeInfo();
		ArgumentNullException.ThrowIfNull(typeInfo.FullName, "Type.FullName cannot be null.");
		ArgumentNullException.ThrowIfNull(typeInfo.Assembly.GetName().Name, "Assembly name cannot be null.");

		localizer = new YamlFileSystemLocalizer(
			typeInfo.Assembly.GetName().Name!,
			typeInfo.FullName,
			_deserializer,
			_resourceFiles,
			_i18nPath,
			_logger);
		_localizerCache[resourceSource.AssemblyQualifiedName!] = localizer;
		return localizer;
	}

	/// <inheritdoc />
	public IStringLocalizer Create(string baseName, string location)
	{
		throw new NotSupportedException();
	}
}