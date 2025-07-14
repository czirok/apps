using System.Globalization;
using YamlDotNet.Serialization;

namespace Yaml.Localization;

/// <summary>
/// Culture item for application
/// </summary>
[YamlSerializable]
public class CultureItem
{
	/// <inheritdoc cref="CultureInfo.Name"/>>
	public string? Name { get; set; }

	/// <summary>
	/// This will be the default culture. Set only one to true.
	/// </summary>
	public bool Default { get; set; }

	/// <summary>
	/// Only the active <see cref="CultureItem"/> is used by the application.
	/// </summary>
	public bool Active { get; set; }

	/// <summary>
	/// Is Right-to-Left (RTL) culture?
	/// </summary>
	public bool Rtl { get; set; }
}