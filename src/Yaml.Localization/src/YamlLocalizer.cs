using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;

namespace Yaml.Localization;

/// <summary>
/// Represents a service that provides localized strings.
/// </summary>
/// <remarks>This type is thread-safe.</remarks>
public abstract partial class YamlLocalizer : IStringLocalizer
{
	readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _cultureCache = new();

	/// <summary>
	/// The base name of the YAML file to search for.
	/// </summary>
	protected readonly string BaseName;

	/// <summary>
	/// The logger for this localizer.
	/// </summary>
	protected readonly ILogger<YamlLocalizer> Logger;

	/// <summary>
	/// Creates a new <see cref="YamlLocalizer"/>.
	/// </summary>
	/// <param name="baseName">The base name of the yaml to search for.</param>
	/// <param name="logger">The <see cref="ILoggerFactory"/>.</param>
	public YamlLocalizer(string baseName, ILogger<YamlLocalizer> logger)
	{
		ArgumentNullException.ThrowIfNull(baseName);
		ArgumentNullException.ThrowIfNull(logger);

		BaseName = baseName;
		Logger = logger;
	}

	/// <inheritdoc />
	public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		=> GetYaml(CultureInfo.CurrentUICulture, includeParentCultures)
		.Select(kvp => new LocalizedString(kvp.Key, kvp.Value)).ToList();

	/// <inheritdoc />
	public LocalizedString this[string name]
	{
		get
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(name);
			var value = GetStringSafely(name);
			return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: BaseName);
		}
	}

	/// <inheritdoc />
	public LocalizedString this[string name, params object[] arguments]
	{
		get
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(name);
			var format = GetStringSafely(name);
			var value = string.Format(CultureInfo.CurrentCulture, format ?? name, arguments);
			return new LocalizedString(name, value, resourceNotFound: format == null, searchedLocation: BaseName);
		}
	}

	/// <summary>
	/// Gets a localized string by its name, safely handling the case where the string may not exist.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="cultureInfo"></param>
	/// <returns></returns>
	public string? GetStringSafely(string name, CultureInfo? cultureInfo = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		var culture = cultureInfo ?? CultureInfo.CurrentUICulture;

		if (Logger.IsEnabled(LogLevel.Debug))
			Log.SearchedLocation(Logger, name, BaseName, culture);

		var yamlCache = _cultureCache.GetOrAdd(culture.Name, _ => GetYaml(culture, true));

		if (yamlCache.TryGetValue(name, out var value)) return value;

		if (Logger.IsEnabled(LogLevel.Warning))
			Log.ResourceNotExist(Logger, name, culture);

		return null;
	}

	/// <summary>
	/// Gets the YAML content for the specified culture.
	/// </summary>
	protected abstract ConcurrentDictionary<string, string> GetYaml(CultureInfo cultureInfo, bool includeParentCultures);
}

public abstract partial class YamlLocalizer
{
	static partial class Log
	{
		[LoggerMessage(1, LogLevel.Debug, $"{nameof(YamlLocalizer)} searched for '{{Key}}' in '{{LocationSearched}}' with culture '{{Culture}}'.", EventName = "SearchedLocation")]
		public static partial void SearchedLocation(ILogger logger, string key, string locationSearched, CultureInfo culture);

		[LoggerMessage(2, LogLevel.Warning, "Resource name '{Name}' with '{Culture}' culture not exist.", EventName = "ResourceNotExist")]
		public static partial void ResourceNotExist(ILogger logger, string name, CultureInfo culture);
	}
}