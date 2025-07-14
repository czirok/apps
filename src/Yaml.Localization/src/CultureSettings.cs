using YamlDotNet.Serialization;

namespace Yaml.Localization;

/// <summary>
/// Culture settings for application
/// </summary>
[YamlSerializable]
public class CultureSettings
{
	public const string DefaultCultureName = "en-US";

	/// <summary>
	/// The endpoint to redirect to after changing the culture.
	/// </summary>
	public string RedirectEndpoint { get; set; } = "culture/set";

	/// <summary>
	/// The cultures supported by the application.
	/// </summary>
	public IList<CultureItem>? Cultures { get; set; } =
	[
		new()
		{
			Name = DefaultCultureName,
			Default = true,
			Active = true
		}
	];

	/// <summary>
	/// Items in language selector
	/// </summary>
	public Dictionary<string, string>? Selector { get; set; }

	/// <summary>
	/// Browser culture cookie name. The default value is: ".lang"
	/// </summary>
	public string CookieName { get; set; } = ".lang";
}
