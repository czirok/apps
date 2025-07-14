using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;
using YamlDotNet.Serialization;

namespace Yaml.Localization;

/// <summary>
/// A localizer that retrieves YAML resources from the file system.
/// </summary>
/// <param name="assemblyName">The assembly name containing the YAML resources.</param>
/// <param name="baseName">The base name of the resource to search for.</param>
/// <param name="deserializer">The <see cref="IDeserializer"/> to use for deserializing YAML content.</param>
/// <param name="resourceFiles">The list of resource files to search for.</param>
/// <param name="i18nPath">The path to the directory containing the YAML files.</param>
/// <param name="logger">The <see cref="ILoggerFactory"/>.</param>
public sealed partial class YamlFileSystemLocalizer(
	string assemblyName,
	string baseName,
	IDeserializer deserializer,
	string[] resourceFiles,
	string i18nPath,
	ILogger<YamlFileSystemLocalizer> logger)
	: YamlLocalizer(baseName, logger)
{
	/// <inheritdoc />
	protected override ConcurrentDictionary<string, string> GetYaml(CultureInfo cultureInfo, bool includeParentCultures)
	{
		var response = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);
		var culture = cultureInfo;

		while (culture.IsNeutralCulture)
		{
			culture = CultureInfo.CreateSpecificCulture(culture.Name);
		}

		while (true)
		{
			string relativeNamespace = BaseName.Substring(assemblyName.Length + 1);
			string relativePath = relativeNamespace.Replace('.', Path.DirectorySeparatorChar);
			string className = relativeNamespace.Split('.').Last();
			string fileName = culture.HasInvariantCultureName()
				? $"{className}.yaml"
				: $"{className}.{culture.Name}.yaml";
			string expectedFilePath = Path.GetFullPath(Path.Combine(i18nPath, relativePath.Replace(Path.GetFileName(relativePath), ""), fileName));

			if (resourceFiles.Any(file => Path.GetFullPath(file).Equals(expectedFilePath, StringComparison.OrdinalIgnoreCase)))
			{
				using var resourceStream = File.OpenRead(expectedFilePath);
				using var reader = new StreamReader(resourceStream!);
				var dictionary = deserializer.Deserialize<Dictionary<string, string>>(reader);
				foreach (var (key, value) in dictionary!)
				{
					if (response.ContainsKey(key)) continue;
					response.TryAdd(key, value);
				}
			}

			if (culture.HasInvariantCultureName()) break;
			if (!includeParentCultures) break;
			culture = culture.Parent;
		}

		return response;
	}
}

public partial class YamlFileSystemLocalizer
{
	static partial class Log
	{
		[LoggerMessage(1, LogLevel.Warning, "Resource name '{Name}' with '{Culture}' culture not exist.", EventName = "ResourceNotExist")]
		public static partial void ResourceNotExist(ILogger logger, string name, CultureInfo culture);

		[LoggerMessage(2, LogLevel.Critical, "Invalid yaml resource format: '{ResourceName}'.", EventName = "InvalidYamlResourceFormat")]
		public static partial void InvalidYamlFormat(ILogger logger, string resourceName);
	}
}
