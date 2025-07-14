using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using YamlDotNet.Serialization;

namespace Yaml.Localization;

/// <summary>
/// Creates a new <see cref="YamlEmbeddedResourceLocalizer"/>.
/// </summary>
/// <param name="assembly">The assembly containing the embedded YAML resources.</param>
/// <param name="baseName">The base name of the resource to search for.</param>
/// <param name="deserializer">The <see cref="IDeserializer"/> to use for deserializing YAML content.</param>
/// <param name="resourceFiles">The list of resource files to search for.</param>
/// <param name="logger">The <see cref="ILoggerFactory"/>.</param>
public sealed partial class YamlEmbeddedResourceLocalizer(
	Assembly assembly,
	string baseName,
	IDeserializer deserializer,
	HashSet<string> resourceFiles,
	ILogger<YamlEmbeddedResourceLocalizer> logger)
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
			var resourceFile = culture.HasInvariantCultureName()
				? BaseName.YamlExt()
				: string.Concat(BaseName, ".", culture.Name, Extensions.Yaml);



			if (resourceFiles.Contains(resourceFile))
			{
				using var resourceStream = assembly.GetManifestResourceStream(resourceFile);
				using var reader = new StreamReader(resourceStream!);
				var dictionary = deserializer.Deserialize<Dictionary<string, string>>(reader);
				if (dictionary?.Count > 0)
				{
					foreach (var (key, value) in dictionary!)
					{
						response.TryAdd(key, value);
					}
				}
			}

			if (culture.HasInvariantCultureName()) break;
			if (!includeParentCultures) break;

			culture = culture.Parent;
		}

		return response;
	}
}
