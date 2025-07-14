using System.Globalization;

namespace Yaml.Localization;

public static class Extensions
{
	public static bool HasInvariantCultureName(this CultureInfo culture)
	{
		return culture.Name == CultureInfo.InvariantCulture.Name;
	}

	public const string Yaml = ".yaml";
	public static string YamlExt(this string file)
	{
		return $"{file}{Yaml}";
	}

	public static Theme ToTheme(this string theme)
	{
		return theme switch
		{
			"light" => Theme.Light,
			"dark" => Theme.Dark,
			_ => Theme.Auto
		};
	}
}